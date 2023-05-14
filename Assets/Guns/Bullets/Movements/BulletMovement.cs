using System;
using UnityEngine;

[RequireComponent(typeof(Physics_Movement))]
[RequireComponent(typeof(DebugableObject))]
public abstract class BulletMovement : MonoBehaviour
{
    protected BaseBulltet _owner;
    protected Physics_Movement _movement;
    protected DebugableObject _debug;

    public abstract void Initialize();

    private void Awake()
    {
        _owner = GetComponent<BaseBulltet>();
        _movement = GetComponent<Physics_Movement>();
        _debug = GetComponent<DebugableObject>();
    }

}
