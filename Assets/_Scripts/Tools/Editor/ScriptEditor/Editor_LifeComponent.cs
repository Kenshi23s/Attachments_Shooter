using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LifeComponent))]
public class Editor_LifeComponent : Editor
{
    // Start is called before the first frame update

    public int damageValue=10;
    public int healValue=10;
    public int overTime=2;
    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox("Metodos", MessageType.Info);
        LifeComponent myTarget = (LifeComponent)target;

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("HealValue :" + healValue.ToString());
        healValue = (int)GUILayout.HorizontalSlider(healValue, 1, myTarget.maxLife);
        EditorGUILayout.Space(10f);

        EditorGUILayout.LabelField("DamageValue :", damageValue.ToString());
        EditorGUILayout.Space(10f);
        damageValue = (int)GUILayout.HorizontalSlider(damageValue, 1, myTarget.maxLife);
        EditorGUILayout.Space(10f);

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("OverTimeValue :", overTime.ToString());
        overTime = (int)GUILayout.HorizontalSlider(overTime, 1, 10);     
        EditorGUILayout.Space(20f);

        //debe haber una forma menos cabeza de hacer esto, no me gusta tantos if asi
        if (GUILayout.Button("TakeDamage"))
        {
            myTarget.TakeDamage(damageValue);
        }

       
        if (GUILayout.Button("Heal"))
        {
            myTarget.Heal(healValue);
        }
      
        EditorGUILayout.Space(10f);

        if (GUILayout.Button("Damage OverTime"))
        {
            myTarget.AddDamageOverTime(damageValue, overTime);
        }

        if (GUILayout.Button("Heal OverTime"))
        {
            myTarget.AddDamageOverTime(damageValue, overTime);
        }

        EditorGUILayout.Space(10f);

        if (GUILayout.Button("Kill"))
        {
            myTarget.TakeDamage(int.MaxValue);
        }


      
        EditorGUILayout.HelpBox("Componente", MessageType.Info);
        DrawDefaultInspector();
    }
}
