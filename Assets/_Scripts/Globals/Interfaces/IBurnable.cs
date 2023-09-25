using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IBurnable
{
    public UnityEvent OnBurn { get; }
    public UnityEvent OnBurnEnd { get; }
    public UnityEvent OnAddBurn { get; }

    public float BurnStacks { get; }
    public void AddStacks(float toAdd, Vector3 from = default);
    public void RemoveStacks(float toRemove);



}
