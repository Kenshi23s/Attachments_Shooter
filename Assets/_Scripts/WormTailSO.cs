using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "wormTail.asset", menuName = "ScriptableObjects/WormTail")]
public class WormTailSO : ScriptableObject
{
    public BoneData[] boneData;
}