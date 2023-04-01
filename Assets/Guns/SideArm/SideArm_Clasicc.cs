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
         BaseBulltet bullet = Get(_myAttachMents.magazineAmmoType);
         bullet.SetGunAndCallback(this, OnHitCallBack);
         bullet.transform.position = _myAttachMents.shootPos.position;
         bullet.transform.forward = _myAttachMents.shootPos.forward;
        
       
    }

   
}
