using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    [Serializable]
    public struct Stock
    {
        public CurrencyShopItemsSO ItemData;
        public int Quantity;
    }
    [field: SerializeField]
    public VendorTable Owner { get; private set; }

    public Transform DispenserPosition;

    [field: SerializeField, Header("Stock")]
    public ShopItem Sample { get; private set; }

    [SerializeField] Transform stockTransform;
    public List<Stock> itemsOnStock = new();
    List<ShopItem> _shopItems = new();

    #region Preview
    public CurrencyShopItemsSO CurrentPreviewItem { get; private set; }
    [Header("Preview"), SerializeField]
    Transform previewParent;
    [SerializeField] Image _iconPreview;
    [SerializeField] TMP_Text _previewName, _previewCost, _previewDescription;

    [field: SerializeField] public Button BuyButton { get; private set; }

    public TMP_Text BalanceText;

    #endregion

    private void Awake()
    {

        if (itemsOnStock.Any())
            foreach (var item in itemsOnStock)
                if (item.Quantity == 0 || item.ItemData == null)
                    itemsOnStock.Remove(item);
        CurrencyManager.OnBalanceChange += AdjustBalanceText;
        AdjustBalanceText();
    }

    void AdjustBalanceText()
    {
        BalanceText.text = $"Current Balance: {CurrencyManager.ActualCurrency}";
    }

    private void Start()
    {
        Owner.OnEnterInventory.AddListener(DisplayItems);
        Owner.OnEnterInventory.AddListener(ClearPreview);

        Owner.OnCloseInventory.AddListener(StopDisplay);

        BuyButton.onClick.AddListener(BuyCurrentItem);
        enabled = false;
    }

    public void DisplayItems()
    {
        
        foreach (var item in itemsOnStock.Where(x => x.ItemData != null && x.Quantity > 0))
        {
            var x = Instantiate(Sample, stockTransform);
            x.SetInfo(this, item.ItemData);
            _shopItems.Add(x);
        }
    }

    public void PreviewItem(CurrencyShopItemsSO item)
    {
        CurrentPreviewItem = item;
        _iconPreview.sprite = item.DisplaySprite;
        _previewName.text = item.ItemName;
        _previewDescription.text = item.Description;
        _previewCost.text = "Cost: " + item.Cost.ToString();
        previewParent.gameObject.SetActive(true);

    }

    public void StopDisplay()
    {
        foreach (var item in _shopItems) Destroy(item);
        _shopItems.Clear();
   
        ClearPreview();
    }

    public void BuyCurrentItem()
    {
        if (CurrentPreviewItem == null) 
        { Debug.Log("No hay item en la preview"); return; } 

        if (!CurrencyManager.ConfirmPurchase(CurrentPreviewItem.Cost)) 
        { Debug.Log("No hay suficiente dinero,no se puede comprar"); return; } 

        Instantiate(CurrentPreviewItem.ItemDisplayed, DispenserPosition.position, Quaternion.identity);
        Debug.Log("Se compro el item!");

        ClearPreview();


    }

    void ClearPreview()
    {
        CurrentPreviewItem = null;
        previewParent.gameObject.SetActive(false);
    }

}
