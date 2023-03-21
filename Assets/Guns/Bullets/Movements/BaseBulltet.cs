using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletProperties;

public struct BulletProperties
{
    public enum Stat
    {
        Speed,
        Damage
    }
    public BulletMovement.Type movementType;
    public Dictionary<Stat, int> stats;
    public GunFather myGun;
    
}

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseBulltet : MonoBehaviour
{
    BulletProperties myProperties;

    Rigidbody _rb;

    Action<HitData> onHit;

    Action<BaseBulltet, string> _poolReturn;
    string poolKey;

    BulletMovement myMovement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        myMovement = BulletMovement.MovementSelected(transform,_rb, myProperties);
    }

    private void Update() => myMovement?.MoveBullet();

    public virtual void Initialize(Action<BaseBulltet,string> _poolReturn,string poolKey, BulletProperties myProperties, Action<HitData> onHit)
    {
        this.myProperties = myProperties;
        this._poolReturn = _poolReturn;
        this.poolKey = poolKey;

        onHit += OnHitEffect;
        this.onHit = onHit;

    }

    public void Hit(IDamagable target)
    {
        int dmgDealt = target.TakeDamage(myProperties.stats[Stat.Damage]);

        HitData hit = new HitData(transform.position,target,dmgDealt, myProperties.myGun);

        onHit?.Invoke(hit);     
    }

    protected abstract void OnHitEffect(HitData hit);

    public void Pause()
    {
        
    }

    public void Resume()
    {
       
    }
}
