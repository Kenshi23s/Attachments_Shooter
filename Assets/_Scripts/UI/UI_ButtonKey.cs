using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_ButtonKey : MonoBehaviour
{
    public TextMeshProUGUI keyValue;
    public TextMeshProUGUI action;

    public void setKeyValue(string keyValue, string action)
    {
        this.keyValue.text = keyValue;
        this.action.text = action;
    }

}
