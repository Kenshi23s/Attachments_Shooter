using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveText.asset", menuName = "ScriptableObjects/ObjectiveText")]
public class ObjectiveTextSO : ScriptableObject
{
    [TextArea(3, 10)] public string text = "Example Text";
    [Range(0.01f, 1)]public float typeSpeed = 0.01f;
    [Min(0)] public float duration = 10f;
    public bool disappearsAfterDuration = true;
    public ObjectiveTextSO nextText;
}
