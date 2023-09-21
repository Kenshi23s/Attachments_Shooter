using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "ShopItem", menuName = "Card/ShopItem")]
public class CurrencyShopItemsSO : ScriptableObject
{
    [field : SerializeField] 
    public int Cost { get; private set; }

    [field : SerializeField]
    public GameObject ItemDisplayed { get; private set; }

    [field: SerializeField]
    public string ItemName { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }

    [field: SerializeField]
    public Sprite DisplaySprite { get; private set; }

    public void SetName(string NewName) => ItemName = NewName;


}
#if UNITY_EDITOR

public class RenameScriptableObject : Editor
{
   

}
#endif
