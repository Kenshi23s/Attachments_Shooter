using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BurnableComponent : MonoBehaviour, IBurnable
{
    #region Variable Declaration
    public float BurnStacks { get; private set; } = 0;

    public float DecresePerTick => DecreaseBurnSpeed * Time.deltaTime;

    [field: SerializeField,Range(0f,10f)]
    public float DecreaseBurnSpeed { get; private set; } = 1;

    [field: SerializeField]
    public UnityEvent OnBurn { get; private set; } = new UnityEvent();

    [field: SerializeField]
    public UnityEvent OnBurnStart { get; private set; } = new UnityEvent();

    [field: SerializeField]
    public UnityEvent OnBurnEnd { get; private set; } = new UnityEvent();

    [field: SerializeField]
    public UnityEvent OnAddBurn { get; private set; } = new UnityEvent();

    [field: SerializeField]
    public UnityEvent OnRemoveBurn { get; private set; } = new UnityEvent();

    public Vector3 InitialBurningPoint { get; private set; }
    #endregion


    private void Awake()
    {
        enabled = false;
    }
    void Update()
    {
        //no se reproduce todo el tiempo, solo cuando "Enable" es verdadero
        Burn();
    }

    public void AddStacks(float toAdd,Vector3 from = default) // vector3.zero
    {
        toAdd = Mathf.Abs(toAdd);
        Debug.Log("Me quemo");
        if (BurnStacks <= 0 && toAdd > 0)
            StartBurning(from);

        BurnStacks += toAdd;
        OnAddBurn?.Invoke();
    }

    public void RemoveStacks(float toRemove)
    {
        toRemove = Mathf.Abs(toRemove) * -1;
        BurnStacks -= toRemove;
        OnRemoveBurn.Invoke();
        if (BurnStacks <= 0)
            EndBurning();

    }

    public void Burn()
    {
        OnBurn.Invoke();
        RemoveStacks(DecresePerTick);
    }

    void StartBurning(Vector3 initialPoint)
    {
        OnBurnStart?.Invoke();
        InitialBurningPoint = initialPoint;
        enabled = true;
    }

    void EndBurning()
    {
        BurnStacks = 0;
        OnBurnEnd.Invoke();
    }
}
