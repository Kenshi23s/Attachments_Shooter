using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public abstract class BaseBulltet : MonoBehaviour,IPausable
{

    Rigidbody _rb;
    // se llama cuando se impacta con un "Target"
    Action <HitData> _hitCallBack;

    // variables para pool
    #region
    //metodo de regreso
    Action<BaseBulltet, string> _poolReturn;
    //key
    string poolKey;
    #endregion

    public GunFather myGun;

    BulletMovement myMovement;
    [SerializeField] BulletMovement.BulletMoveType myType;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // creo el tipo de movimiento q voy a usar
        myMovement = BulletMovement.MovementSelected(transform,_rb, myType);
        //test
        myMovement.SetSpeed(12);
    }

    private void Update() => myMovement?.MoveBullet();

    public virtual void Initialize(Action<BaseBulltet,string> _poolReturn,string poolKey)
    {    
        this._poolReturn = _poolReturn;
        this.poolKey = poolKey;     
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="myGun"></param>
    /// <param name="_hitCallBack"></param>
   public void SetGunAndDispatch(GunFather myGun,Action<HitData> _hitCallBack)
   {
        this.myGun = myGun;
        myGun.OnHit += OnHitEffect;
        this._hitCallBack = _hitCallBack;
    

        transform.position = myGun.attachMents.shootPos.position;
        transform.forward = myGun.attachMents.shootPos.forward;



    }

    public void Hit(IDamagable target)
    {
        int dmgDealt = target.TakeDamage(myGun.damageManager.actualDamage);

        HitData hit = new HitData(transform.position,target,dmgDealt, myGun);

        _hitCallBack(hit);
     
    }

    protected abstract void OnHitEffect(HitData hit);


    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out IDamagable x ))
        {
            Hit(x);
        }

        //me desuscribo de el evento y borro la referencia al callback
        myGun.OnHit -= OnHitEffect;
        _hitCallBack=null;
        _poolReturn.Invoke(this, poolKey);
    }
    public void Pause()
    {
        
    }

    public void Resume()
    {
       
    }
}
