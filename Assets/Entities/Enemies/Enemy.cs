using System;
using UnityEngine;
[RequireComponent(typeof(LifeComponent))]
[RequireComponent(typeof(DebugableObject))]
public abstract class Enemy : MonoBehaviour
{
    [NonSerialized] public DebugableObject _debug;
    [NonSerialized] public LifeComponent health;
    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        health= GetComponent<LifeComponent>();
        ArtificialAwake();
    }
    public abstract void ArtificialAwake();
}
