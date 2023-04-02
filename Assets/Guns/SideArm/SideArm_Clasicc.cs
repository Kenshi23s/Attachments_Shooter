using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideArm_Clasicc : SideArm
{
    Func<string,BaseBulltet> Get;
    Action<GunFather, Action<HitData>> Set;
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
         Debug.Log("Shoot");
         BaseBulltet bullet = Get(attachMents.magazineAmmoType);
         bullet.SetGunAndCallback(this, OnHitCallBack);
         bullet.transform.position = attachMents.shootPos.position;
         bullet.transform.forward = attachMents.shootPos.forward;
        
       
    }

   
}
