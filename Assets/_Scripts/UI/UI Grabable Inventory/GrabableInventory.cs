using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_GrabableItem;

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

    private void Awake()
    {
        GHandler.OnEquip.AddListener(UpdateUI);
        GHandler.OnUnEquip.AddListener(UpdateUI);
        GHandler.OnGrab.AddListener(UpdateUI);
        GHandler.onThrow.AddListener(UpdateUI);
    }


    void UpdateUI()
    {
        if (GHandler.Inventory.Count > GrabableItemsIcons.Count)
        {
            int quantity = GHandler.Inventory.Count - GrabableItemsIcons.Count;
            for (int i = 0; i < quantity; i++)
            {
                var x = Instantiate(Sample, CanvasInventory);
                GrabableItemsIcons.Add(x);
            }
        }

        for (int i = 0; i < GrabableItemsIcons.Count; i++)
        {
            if (i == GHandler.EquippedIndex)           
                GrabableItemsIcons[i].Focus();
            else
                GrabableItemsIcons[i].UnFocus();


            var x = new IconParameters();
            x.ItemOwner = GHandler.Inventory[i];
            x.name = GHandler.Inventory[i].Transform.name;
            x.index = i;
            //esto lo habria que rellenar cuando haya icono
            x.icon = default;
            GrabableItemsIcons[i].UpdateUI_Item(x);


        }
    }



}
