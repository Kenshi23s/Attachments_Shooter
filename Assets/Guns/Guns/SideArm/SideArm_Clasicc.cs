using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideArm_Clasicc : AutomaticGun
{
    Func<int,BaseBulltet> Get;
   // Action<GunFather, Action<HitData>> Set;
    protected override void OptionalInitialize()
    {
        base.OptionalInitialize();
      
    }
    private void Start()
    {
        Get = (key) => Bullet_Manager.instance.GetProjectile(Bullet_Manager.defaultBulletKey);
        
    }


    public override void Shoot()
    {
        _debug.Log($"Shot fired by{gameObject.name}");
         BaseBulltet bullet = Get(attachmentHandler.magazineAmmoType);
        if (bullet!=null)
        {
            bullet.SetGunAndDispatch(this, OnHitCallBack);
        }
        else
        {
            Debug.LogError("Error, bala == null, la pool no esta integrada o la bala default cambio");
        }          
    }

    public override bool ShootCondition()
    {
        // aca chequearia las condiciones para disparar
        return _rateFireHandler.canShoot;
    }
}
