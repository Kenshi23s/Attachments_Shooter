using FacundoColomboMethods;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using System.Collections;
using System.Linq;

public class Trap_DmgOvertime : MonoBehaviour
{
   [SerializeField] DecalProjector _floorProjection;

   [SerializeField]protected float _dmg;

    #region Events
    public event Action<IDamagable> onEnter;
    public event Action<IDamagable> onStay;
    public event Action<IDamagable> onExit;
    #endregion

    protected List<IDamagable> dmgList;


    public void Initialize()
    {
       
    }

    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
        {
            if (dmgList.Count >= 1) StartCoroutine(CoroutineMake());
            dmgList.CheckAndAdd(x); onEnter?.Invoke(x);
        }                                
    }
    List<IDamagable> aux;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
            if (dmgList.Contains(x)) aux.CheckAndAdd(x);
    }

    IEnumerator CoroutineMake()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (true)
        {
            yield return wait;
            foreach (var item in aux) onStay(item);
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
        {
            onExit?.Invoke(x); dmgList.CheckAndRemove(x); if (!dmgList.Any()) StopCoroutine(CoroutineMake());          
        }         
    }
}
