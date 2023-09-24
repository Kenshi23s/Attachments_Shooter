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

    //el canvas group me permite bajarle la opacidad a todos los hijos del GO
    public CanvasGroup MyCanvasGroup { get; private set; }


  

    public struct IconParameters
    {
        public int index;
        public IGrabable ItemOwner;
        public Sprite icon;
        public string name;
    }

    private void Awake()
    {
        MyCanvasGroup = GetComponent<CanvasGroup>();

    }

    public void SetOpacity(float x) => MyCanvasGroup.alpha = x;

    public void SetOwner(GrabableInventory x) => InventoryOwner = x;


    public void UpdateUI_Item(IconParameters x)
    {
        IndexText.text = x.index.ToString();
        TextName.text = x.name;
        myParameters = x;
        SetOpacity(1);
    }

    public void Focus()
    {
        transform.localScale = Vector3.one * InventoryOwner.ScaleFocusBy;
        UI.color = InventoryOwner.FocusColor;
    }

    public void UnFocus()
    {
        transform.localScale = Vector3.one;
        UI.color = InventoryOwner.UnfocusColor;
    }
}
