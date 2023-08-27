using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]

public class HitVFX : MonoBehaviour, IHitFeedback
{
    [SerializeField]ParticleHolder _vfxPrefab;
    DebugableObject _debug;
    Func<ParticlePool> GetPool;
    int key;
    public const float PositionOffSet = 0.25f; 
    

    private void Awake()
    {
        if (_vfxPrefab == null) Destroy(this);
        _debug = GetComponent<DebugableObject>();    
    }

    private void Start()
    {
        if (GameManager.instance == null)
            return;

        GetPool = () => GameManager.instance.vfxPool;
        key = GetPool().CreateVFXPool(_vfxPrefab);
        enabled = false;
    } 

   
    public void FeedbackHit(Vector3 hitPoint,Vector3 hitDir)
    {
        _debug.Log("HIT!, muestro feedback");
        var aux = GetPool().GetVFX(key);
        aux.transform.position = hitPoint - hitDir * PositionOffSet;  
        aux.transform.forward = hitDir;

        StartCoroutine(aux.CooldownDecrease());
    }

}


