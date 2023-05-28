using System;
using UnityEngine;
[RequireComponent(typeof(LifeComponent))]
[RequireComponent(typeof(DebugableObject))]
public abstract class Enemy : MonoBehaviour
{
    [NonSerialized] public DebugableObject debug;
    [NonSerialized] public LifeComponent health;
    private void Awake()
    {
        debug = GetComponent<DebugableObject>();
        health= GetComponent<LifeComponent>();
        ArtificialAwake();
    }
    public abstract void ArtificialAwake();
}
