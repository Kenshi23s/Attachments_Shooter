using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
    
[CustomEditor(typeof(sc_AVRP))]
public class sc_InspectorAVRP : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space(20);

        sc_AVRP myAVRP = (sc_AVRP) target;


        //Afectan al total de Probes
        #region Linea_1

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Probes"))
        {
            myAVRP.CreateProbes(myAVRP.GetCornerBox(myAVRP.transform.position, -myAVRP.boxSize), myAVRP.GetCornerBox(myAVRP.transform.position, myAVRP.boxSize));
        }

        if (GUILayout.Button("Clean"))
        {
            myAVRP.CleanAllProbes();
        }

        if (GUILayout.Button("Remove"))
        {
            myAVRP.RemoveAllProbes();
        }

        GUILayout.EndHorizontal();

        #endregion

        #region Linea_2

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Detect"))
        {
            myAVRP.DetectExistingProbes();
        }

        GUILayout.EndHorizontal();

        #endregion

        EditorGUILayout.HelpBox("    Probes: " + myAVRP.probes.Count, MessageType.Info);

        //Afectan a Probes individuales
        #region Linea_3

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add"))
        {
            myAVRP.AddIndividualProbe();
        }

        if (GUILayout.Button("Remove"))
        {
            myAVRP.RemoveIndividualProbe(Selection.activeGameObject);
        }

        GUILayout.EndHorizontal();

        #endregion

        //mostrar estadisticas
        EditorGUILayout.Space(5);

        EditorGUILayout.Space(40);

        base.OnInspectorGUI();

    }
}
