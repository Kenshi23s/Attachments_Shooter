using System;
using UnityEngine;
[RequireComponent(typeof(LifeComponent))]
public abstract class Entity : MonoBehaviour
{

    [SerializeField] protected LifeComponent lifeHandler;
    private void Awake()
    {
        lifeHandler= GetComponent<LifeComponent>();
    }
}
