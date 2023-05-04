using System;
using UnityEngine;
[RequireComponent(typeof(LifeComponent))]
public abstract class Entity : MonoBehaviour
{

    [SerializeField] public LifeComponent lifeHandler;
    protected virtual void Awake()
    {
        lifeHandler= GetComponent<LifeComponent>();
        lifeHandler.Initialize();
    }
}
