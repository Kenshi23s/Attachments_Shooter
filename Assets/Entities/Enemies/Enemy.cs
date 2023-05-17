using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LifeComponent))]
[RequireComponent(typeof(DebugableObject))]
public abstract class Enemy :MonoBehaviour
{
    [NonSerialized] public DebugableObject debug;
}
