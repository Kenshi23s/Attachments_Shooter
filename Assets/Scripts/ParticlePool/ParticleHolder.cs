using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHolder : MonoBehaviour
{
    [SerializeField, Range(0, 10)] float _totalDuration;
    Action<ParticleHolder> returnToPool;

    public void InitializeParticle(Action<ParticleHolder> returnToPool)
    {
        this.returnToPool = returnToPool;
    }

    private void OnEnable()
    {
        StartCoroutine(CooldownDecrease());
    }

    IEnumerator CooldownDecrease()
    {
        yield return new WaitForSeconds(_totalDuration);
        returnToPool(this);
    }

}
