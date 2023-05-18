using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(BoxCollider))]
public class HealItem : MonoBehaviour
{
    DebugableObject _debug;
    [SerializeField,Range(0.1f,30)] float HealAmount;
    [SerializeField,Range(0.1f, 30)] float timeToHeal;
    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        GetComponent<BoxCollider>().isTrigger = true;
        enabled= false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out IHealable target))
        {
            //target.AddHealOverTime();
        }
    }
}
