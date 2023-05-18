using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Enemy_AirTurret))]
public class Editor_AirTurret : Editor
{

    public override void OnInspectorGUI()
    {


        Enemy_AirTurret myTarget = (Enemy_AirTurret)target;

        EditorGUILayout.HelpBox("Informacion Live", MessageType.Info);

        EditorGUILayout.LabelField("Misile Cooldown Left", myTarget.ActualMisileCD.ToString());

        EditorGUILayout.LabelField("Volley Cooldown Left", myTarget.ActualVolleyCD.ToString());

        EditorGUILayout.LabelField("Misiles Left", myTarget.MisilesLeft.ToString());
        EditorGUILayout.Space(10f);
        DrawDefaultInspector();
    }
    
}
