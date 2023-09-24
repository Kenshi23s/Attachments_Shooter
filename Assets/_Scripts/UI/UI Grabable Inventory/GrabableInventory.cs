using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_GrabableItem;
using static UnityEditor.Progress;

public class GrabableInventory : MonoBehaviour
{
    [field: SerializeField, Range(1f, 2f)]
    public float ScaleFocusBy { get; private set; } = 1.25f;

    public Color FocusColor, UnfocusColor;



    [field: SerializeField]
    public UI_GrabableItem Sample { get; private set; }

    [field: SerializeField]
    public Transform CanvasInventory { get; private set; }

    public GrabableHandler GHandler { get; private set; }

    List<UI_GrabableItem> GrabableItemsIcons = new();

    public float ItemTimeActive { get; private set; }
    public float DecreaseOpacitySpeed;
    float _currentTimeActive;

    private void Awake()
    {
        GHandler = transform.root.GetComponent<GrabableHandler>();
        GHandler.OnEquip.AddListener(UpdateUI);
        GHandler.OnUnEquip.AddListener(UpdateUI);
        GHandler.OnGrab.AddListener(UpdateUI);
        GHandler.onThrow.AddListener(UpdateUI);
    }

    private void Update()
    {

    }


    IEnumerator FadeOut()
    {
        while (_currentTimeActive > 0)
        {
            foreach (var item in GrabableItemsIcons)
                item.SetOpacity(_currentTimeActive / ItemTimeActive);

            _currentTimeActive -= DecreaseOpacitySpeed * Time.deltaTime;
            yield return null;
        }
        foreach (var item in GrabableItemsIcons)
            item.SetOpacity(0);
    }

    void UpdateUI()
    {
        if (GHandler.Inventory.Count > GrabableItemsIcons.Count)
            CreateMoreIcons(GHandler.Inventory.Count - GrabableItemsIcons.Count);


        for (int i = 0; i < GrabableItemsIcons.Count; i++)
        {
            if (i == GHandler.EquippedIndex)
                GrabableItemsIcons[i].Focus();
            else
                GrabableItemsIcons[i].UnFocus();


            var x = MakeParameters(GHandler.Inventory[i]);
            x.index = i;

            GrabableItemsIcons[i].UpdateUI_Item(x);
        }
    }

    IconParameters MakeParameters(IGrabable item)
    {
        var y = new IconParameters();
        y.ItemOwner = item;
        y.name = item.Transform.name;

        //esto lo habria que rellenar cuando haya icono
        y.icon = default;
        return y;
    }

    void CreateMoreIcons(int quantity)
    {

        for (int i = 0; i < quantity; i++)
        {
            var x = Instantiate(Sample, CanvasInventory);
            x.SetOwner(this);
            GrabableItemsIcons.Add(x);
        }
    }
}
