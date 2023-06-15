using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "wormTail.asset", menuName = "Inventory/Item")]
public class WormTailSO : ScriptableObject
{
    public BoneData[] boneData;
}