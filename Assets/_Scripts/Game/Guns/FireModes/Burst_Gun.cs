using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Burst_Gun : Gun
{
    #region Variables
    [Header("Burst Parameters")]
  
    [SerializeField,Range(2,5)]
    protected int bulletsPerBurst;
    [SerializeField, Range(0.1f, 1f),Tooltip("Cooldown entre bala y bala en una rafaga")]
    protected float bulletCooldown;
    [SerializeField, Range(0.1f, 1f), Tooltip("Cooldown entre rafaga y rafaga")]
    protected float burstCooldown;
    #endregion
    public int burstCount { get; private set; }

    public event Action<int> burstNumber;
    public abstract void ShootOnBurst();

    public override void Shoot() => StartCoroutine(ShootBurst());

    public override bool ShootCondition() => canShoot;
   
    IEnumerator ShootBurst()
    {
        canShoot = false;
        burstCount = 0;
        while (burstCount < bulletsPerBurst)
        {
            ShootOnBurst(); CallOnShootEvent();
            burstCount++;
            burstNumber?.Invoke(burstCount);
            _debug.Log("Bullet "+ burstCount + " de "+ bulletsPerBurst);
            yield return new WaitForSeconds(bulletCooldown);
            

        }
       
        yield return new WaitForSeconds(burstCooldown);
        canShoot = true;
    }

    // es lo mismo solo q saco el ONSHOOT, pq no lo puedo invocar como hijo
    public override void Trigger()
    {
        if (ShootCondition())
        {
            Shoot();
            //onShoot?.Invoke();          
        }
    }
   



}
