using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurrencyShopItemsSO))]
public class Editor_ShopItemSO : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //lo voy a rechequear otro dia
        return;
        CurrencyShopItemsSO scriptableObject = (CurrencyShopItemsSO)target;

        EditorGUI.BeginChangeCheck();

        // Muestra el nombre actual de la variable
        string currentName = scriptableObject.ItemName;

        // Permite que el usuario modifique el nombre
        //currentName = EditorGUILayout.TextField("Name", currentName);

        if (EditorGUI.EndChangeCheck())
        {
            // Guarda el nuevo nombre en la variable
            scriptableObject.SetName(currentName);


            // Cambia el nombre del archivo del ScriptableObject
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(scriptableObject), currentName + " Item SO");
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(target);
        }

        
    }
}
