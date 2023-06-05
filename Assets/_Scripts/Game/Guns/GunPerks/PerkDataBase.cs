#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static class PerkDataBase 
{
    public static string PerksPath => Application.dataPath + "/Json/Perks/";

    public static Perk GetRandomPerk(this Gun gunType)
    {
        List<string> Incompatibles = new List<string>();
        string[] fileEntries = Directory.GetFiles(PerksPath);
        int randomPerk = Random.Range(0, fileEntries.Length);
        //despues habria q preguntar que si el perk no es compatible
        //que haga recursion hasta encontrar uno compatible(se podria usar linq para filtrar)
        return GetOneAtPath(fileEntries[randomPerk]);
    }

    
    public static Perk[] GetAtPath(string path)
    {

        List<Object> list = new List<Object>();
        string[] fileEntries = Directory.GetFiles(path);
        foreach (string fileName in fileEntries)
        {
            //debugear esto para ver que hace
            int index = fileName.LastIndexOf("/");
            string localPath = path;

            if (index > 0)
                localPath += fileName.Substring(index); //debugear esto para ver que hace

            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(Perk));

            if (t != null) list.Add(t);
        }
        Perk[] result = new Perk[list.Count];
        for (int i = 0; i < list.Count; i++)
            result[i] = (Perk)list[i];

        return result;
    }

    public static Perk GetOneAtPath(string path)
    {
        string[] fileEntries = Directory.GetFiles(path);
        foreach (string fileName in fileEntries)
        {
            int index = fileName.LastIndexOf("/");
            string localPath = path;

            if (index > 0)
                localPath += fileName.Substring(index);

            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(Perk));

            if (t != null) return (Perk)t;

        }
        return null;
      
    }
}
#endif
