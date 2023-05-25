using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_buttonsManager : MonoBehaviour
{
    public SerializedDictionary<string,string> keyValuePairs = new SerializedDictionary<string,string>();

    public UI_ButtonKey sample;

    List<UI_ButtonKey> buttonKeys = new List<UI_ButtonKey>();

    private void Awake()
    {
        foreach (var pair in keyValuePairs)
        {
            UI_ButtonKey ui_buttonKey = Instantiate(sample, this.transform);
            ui_buttonKey.setKeyValue(pair.Key, pair.Value);
            buttonKeys.Add(ui_buttonKey);
        }
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            foreach(var p in buttonKeys)
            {
                p.gameObject.SetActive(!p.isActiveAndEnabled);
            }
        }
    }
}
