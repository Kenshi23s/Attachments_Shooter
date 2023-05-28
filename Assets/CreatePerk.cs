using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Drawing;

public class CreatePerk : MonoBehaviour
{
    public Perk PerkToSave;

    public void CreateJson()
    {
        if (PerkToSave == null) return;

        string aux = JsonUtility.ToJson(PerkToSave);

        File.WriteAllText(Application.dataPath + "/Json/Perks/"+PerkToSave.GetName()+".txt", aux);
        PerkToSave = null;
    }

    //C:\Users\Facundo\Documents\Seminario Scavenger\Assets\Json\Perks
}
[CustomEditor(typeof(CreatePerk))]
public class CreatePerkEditor : Editor
{
    

    public override void OnInspectorGUI()
    {
        CreatePerk myTarget = (CreatePerk)target;

        if (GUILayout.Button("CreatePerk"))
        {
            myTarget.CreateJson();
        }
        base.OnInspectorGUI();
    }
}
