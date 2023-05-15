using System;
using System.Collections;
using UnityEngine;

public class ParticleHolder : MonoBehaviour
{
  

    [SerializeField, Range(0, 10)] float _totalDuration;
    Action<ParticleHolder,int> _returnToPool;
    int key;
    public void InitializeParticle(Action<ParticleHolder,int> _returnToPool,int key)
    {
        this._returnToPool = _returnToPool;
        this.key = key;
    }  

    private void OnEnable() => StartCoroutine(CooldownDecrease());

    IEnumerator CooldownDecrease()
    {
        yield return new WaitForSeconds(_totalDuration);
        _returnToPool(this,key);
    }

}
