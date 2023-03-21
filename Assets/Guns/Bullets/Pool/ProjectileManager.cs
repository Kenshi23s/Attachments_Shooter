using System;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileManager : MonoBehaviour
{
    BulletPool pool = new BulletPool();
    public static ProjectileManager instance;

    private void Awake()
    {
        instance = this;
        
    }

    public BaseBulltet GetProjectile(string bulletKey,BulletProperties properties,Action<HitData> OnHit) 
               => pool.AskForProjectile(bulletKey, properties, OnHit);
   
}
