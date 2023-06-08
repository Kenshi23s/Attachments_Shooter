using FacundoColomboMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
        {
            dmgList.CheckAndAdd(x); onEnter?.Invoke(x);
        }                                
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
            if (dmgList.Contains(x)) onStay?.Invoke(x);      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable x))
        {
            onExit?.Invoke(x); dmgList.CheckAndRemove(x);
        }         
    }
}
