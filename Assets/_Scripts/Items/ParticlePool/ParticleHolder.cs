using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class ParticleHolder : MonoBehaviour
{

    DebugableObject _debug;
    [SerializeField, Range(0, 10)] float _totalDuration;
    Action<ParticleHolder,int> _returnToPool;
    public event Action OnFinish;

    int key;
    private void Awake()
    {
        _debug=GetComponent<DebugableObject>();
        enabled=false;
    }
    public void InitializeParticle(Action<ParticleHolder,int> _returnToPool,int key)
    {
        this._returnToPool = _returnToPool;
        this.key = key;
        
    }  
    
    //private void OnEnable() => StartCoroutine(CooldownDecrease());

    public IEnumerator CooldownDecrease()
    {
        yield return new WaitForSeconds(_totalDuration);
        OnFinish?.Invoke();
        _returnToPool(this,key);
    }

}
