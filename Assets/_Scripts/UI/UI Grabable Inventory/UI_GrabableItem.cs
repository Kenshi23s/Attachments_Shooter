using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GrabableItem : MonoBehaviour
{
    public TextMeshProUGUI IndexText,TextName;
    public Image Icon;
    public GrabableInventory InventoryOwner { get; private set; }
    public Image UI;
    IconParameters myParameters = new();



    public struct IconParameters
    {
        public int index;
        public IGrabable ItemOwner;
        public Sprite icon;
        public string name;
    }

    
    public void UpdateUI_Item(IconParameters x)
    {
        IndexText.text = x.index.ToString();
        TextName.text = x.name;
        myParameters = x;
    }

    public void Focus()
    {
        transform.localScale = Vector3.one * InventoryOwner.ScaleFocusBy;
    }

    public void UnFocus()
    {
        transform.localScale = Vector3.one;
    }
}
