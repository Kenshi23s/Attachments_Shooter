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
        BoxCollider x = GetComponent<BoxCollider>();
        x.size = new Vector3(radius, x.size.y,radius);
        _floorProjection.size = x.size;
    }

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        GetComponent<BoxCollider>().isTrigger = true;
        Initialize();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable target))
        {
            bool hasAny = dmgList.Any();
            dmgList.CheckAndAdd(target); onEnter?.Invoke(target);
            if (!hasAny) StartCoroutine(CoroutineMake());        
        }                                
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
            if (dmgList.Contains(x)) aux.CheckAndAdd(x);
    }

    IEnumerator CoroutineMake()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (dmgList.Any())
        {
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
        BoxCollider x = GetComponent<BoxCollider>();
        x.size = new Vector3(radius, x.size.y, radius);
        _floorProjection.size = x.size;
    }
}
