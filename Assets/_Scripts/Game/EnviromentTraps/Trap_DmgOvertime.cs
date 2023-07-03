using FacundoColomboMethods;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(DebugableObject))]
public class Trap_DmgOvertime : MonoBehaviour
{
    [SerializeField] DecalProjector _floorProjection;
    [SerializeField] ParticleSystem _bubbleParticles;

    [SerializeField] protected float _dmg;

    [SerializeField] float radius;

    DebugableObject _debug;
    #region Events
    public event Action<IDamagable> onEnter;
    public event Action<IDamagable> onStay;
    public event Action<IDamagable> onExit;
    #endregion

    protected List<IDamagable> dmgList = new List<IDamagable>();

    List<IDamagable> aux = new List<IDamagable>();

    public void Initialize()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(radius, collider.size.y, radius);
        _floorProjection.size = new Vector3(radius, radius, _floorProjection.size.z);
    }

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        GetComponent<BoxCollider>().isTrigger = true;
        _bubbleParticles = GetComponentInChildren<ParticleSystem>();
        Initialize();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable target))
        {
            bool hasAny = dmgList.Any();
            dmgList.CheckAndAdd(target); onEnter?.Invoke(target);
            aux.CheckAndAdd(target);
            if (!hasAny) StartCoroutine(CoroutineMake());        
        }                                
    }

  

    IEnumerator CoroutineMake()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (dmgList.Any())
        {
            yield return new WaitUntil(ScreenManager.IsPaused);
            _debug.Log("on Coroutine :D");
            yield return wait;
            foreach (var item in aux) onStay(item);
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
        {
            onExit?.Invoke(x); 
            if (dmgList.Contains(x))
            {
                dmgList.Remove(x);
                aux.Remove(x);
            }
            if (!dmgList.Any()) StopCoroutine(CoroutineMake());          
        }         
    }
    private void OnValidate()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(radius, collider.size.y, radius);
        _floorProjection.size = new Vector3(radius, radius, _floorProjection.size.z);
    }
}
