using System;
using System.Diagnostics;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class HitVFX : MonoBehaviour, IHitFeedback
{
    [SerializeField]ParticleHolder _vfxPrefab;
    DebugableObject _debug;
    Func<ParticlePool> GetPool;
    int key;

    private void Awake()
    {
        if (_vfxPrefab == null) Destroy(this);
        _debug= GetComponent<DebugableObject>();
      
    }
    private void Start()
    {
        GetPool = () => GameManager.instance.vfxPool;

        key = GetPool().CreateVFXPool(_vfxPrefab);
        enabled= false;
    } 
   
    public void FeedbackHit(Vector3 hitPoint,Vector3 hitDir)
    {
        _debug.Log("hit,muestro feedback");
        var aux = GetPool().GetVFX(key);
        aux.transform.position = hitPoint;  
        aux.transform.forward= hitDir;
    }
}


