using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(sc_guidDesingManager))]
public class InspectorGuidD : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space(20);

        sc_guidDesingManager guidManager = (sc_guidDesingManager)target;


        //Afectan al total de Probes
        #region Linea_1

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Create"))
        {
            guidManager.CreateGuide(guidManager.distancePerGuide);
        }

        if (GUILayout.Button("Update"))
        {
            guidManager.UpdateGuides(guidManager.distancePerGuide);
        }

        GUILayout.EndHorizontal();

        #endregion


        EditorGUILayout.Space(40);

        base.OnInspectorGUI();
    }
}

