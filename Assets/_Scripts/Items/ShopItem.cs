using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(Button))]
public class ShopItem : MonoBehaviour
{
    public CurrencyShopItemsSO CardInfo { get; private set; }

    [field:SerializeField]  public TextMeshProUGUI Name {get; private set; }

    [field:SerializeField]  public Image DisplayImage{ get; private set; }
    public Button ItemButton { get; private set; }

    public ShopHandler Owner { get; private set; }
    private void Awake()
    {
        ItemButton = GetComponent<Button>();
    }

    public void SetInfo(ShopHandler owner, CurrencyShopItemsSO newCardInfo)
    {
        ItemButton.onClick.RemoveListener(PreviewMySelf);
        if (owner == null || newCardInfo == null) 
        { Debug.Log("No hay owner o la carta es null, me destruyo"); Destroy(gameObject); } 

        Owner = owner;
        CardInfo = newCardInfo;
        Name.text = CardInfo.ItemName;
        DisplayImage.sprite = newCardInfo.DisplaySprite;
        ItemButton.onClick.AddListener(PreviewMySelf);
        
    }

    void PreviewMySelf() => Owner.PreviewItem(CardInfo);



}
