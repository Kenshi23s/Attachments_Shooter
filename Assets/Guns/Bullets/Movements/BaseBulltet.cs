using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public abstract class BaseBulltet : MonoBehaviour,IPausable
{
    

    Rigidbody _rb;

    Action <HitData> CallBack;

    Action<BaseBulltet, string> _poolReturn;
    string poolKey;

    public GunFather myGun;

    BulletMovement myMovement;
    [SerializeField] BulletMovement.BulletMoveType myType;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        myMovement = BulletMovement.MovementSelected(transform,_rb, myType);
    }

    private void Update() => myMovement?.MoveBullet();

    public virtual void Initialize(Action<BaseBulltet,string> _poolReturn,string poolKey)
    {
       
        this._poolReturn = _poolReturn;
        this.poolKey = poolKey;

      

    }

   public void SetGunAndCallback(GunFather myGun,Action<HitData> HitCallback)
   {
        this.myGun = myGun;
        myGun.OnHit += OnHitEffect;


   }

    public void Hit(IDamagable target)
    {
        int dmgDealt = target.TakeDamage(myGun._actualDamage);

        HitData hit = new HitData(transform.position,target,dmgDealt, myGun);

        CallBack(hit);
     
    }

    protected abstract void OnHitEffect(HitData hit);


    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out IDamagable x ))
        {
            Hit(x);
        }

        myGun.OnHit -= OnHitEffect;
        _poolReturn.Invoke(this, poolKey);
    }
    public void Pause()
    {
        
    }

    public void Resume()
    {
       
    }
}
