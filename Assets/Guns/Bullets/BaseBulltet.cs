using System;
using UnityEngine;

[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PoolableObject<BaseBulltet>))]
public abstract class BaseBulltet : MonoBehaviour
{

    protected DebugableObject _debug;
    // se llama cuando se impacta con un "Target"
    Action <HitData> _hitCallBack;

    PoolableObject<BaseBulltet> _poolableObject;
    [NonSerialized] Gun myGun;

    protected abstract void OnHitEffect(HitData hit);

    public void Initialize(Action<BaseBulltet, int> ReturnMethod,int key) => _poolableObject.Initialize(ReturnMethod,key);

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        _poolableObject = new PoolableObject<BaseBulltet>();
        if (_poolableObject==null)
        {
            _debug.ErrorLog("_Poolable Object == null");
        }
    }
    private void Start()
    {
        _poolableObject.onPoolEnter += OnReturnToPool;
    }
    protected void ReturnToPool()  => _poolableObject.EnterPool();

    public void ExitPool() => _poolableObject.ExitPool();

    void OnReturnToPool()
    {
        myGun.onHit -= OnHitEffect;
        _hitCallBack = null;
    }


    public void SetGunAndDispatch(Gun myGun,Action<HitData> _hitCallBack)
    {
         this.myGun = myGun;
         myGun.onHit += OnHitEffect; 
        
         this._hitCallBack = _hitCallBack;
    
         Transform shootPos = myGun.attachmentHandler._shootPos;
    
         transform.position = shootPos.position; transform.forward = shootPos.forward;
    }

    protected void Hit(IDamagable target)
    {        
        DamageData dmgData = target.TakeDamage(myGun.damageHandler.currentDamage);

        HitData hit = new HitData(transform.position, dmgData, myGun);

        _hitCallBack?.Invoke(hit);   
    }

    

   
}
