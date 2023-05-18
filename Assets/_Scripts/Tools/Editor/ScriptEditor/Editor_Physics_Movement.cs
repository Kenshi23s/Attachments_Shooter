using UnityEditor;
[CustomEditor(typeof(Physics_Movement))]
public class Editor_Physics_Movement : Editor
{
    public override void OnInspectorGUI()
    {
        Physics_Movement myTarget = (Physics_Movement)target;

        EditorGUILayout.HelpBox("Info", MessageType.Info);

        EditorGUILayout.LabelField("ActualVelocity", myTarget.velocity.magnitude.ToString());

        EditorGUILayout.Space(10f);
        DrawDefaultInspector();
    }
}
