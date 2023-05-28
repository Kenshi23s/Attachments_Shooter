using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class JSON_PerkWritter : EditorWindow
{
    [SerializeField]
    public Perk PerkToSave;
    [MenuItem("Window/Perk Saver")]
    public static void ShowWindow()
    {
        GetWindow<JSON_PerkWritter>("Save Perk");
    }

    private void OnGUI()
    {
        GUILayout.Label("Esta ventana se usa para guardar perks y despues poderlos adquirir en el juego", EditorStyles.boldLabel);
        if (GUILayout.Button("CreatePerk"))
        {
            CreateJson();
        }
    }
    public void CreateJson()
    {
        if (PerkToSave == null) return;
      
        string aux = JsonUtility.ToJson(PerkToSave);

        File.WriteAllText(Application.dataPath + "Assets/_Scripts/Game/Guns/GunPerks/Perks/JsonPerks/Content" + PerkToSave.name+".txt", aux);
    }
}
