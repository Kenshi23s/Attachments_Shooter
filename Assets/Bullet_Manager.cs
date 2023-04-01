using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoSingleton<Bullet_Manager>
{
    BulletPool mypool;
    static BaseBulltet _defaultBulletPrefab;
    [SerializeField] BaseBulltet _defaultBulletSample;

    public static string defaultBulletKey => _defaultBulletKey;
    static string _defaultBulletKey = "Default";


    // Start is called before the first frame update
    protected override void ArtificialAwake()
    {
        mypool = new BulletPool();
        _defaultBulletPrefab = _defaultBulletSample;
        mypool.Initialize();
        mypool.CreateBullet(_defaultBulletKey, _defaultBulletPrefab);
    }

     public BaseBulltet GetProjectile(string key) 
        => mypool.AskForProjectile(key);

    public void CreateBullet(string key,BaseBulltet bullet)
      => mypool.CreateBullet(key, bullet);

    //public BaseBulltet SetProjectile(string key, BulletProperties properties, Action<HitData> onHit)
    //    => mypool.AskForProjectile(key, properties, onHit);

    //public void CreateBullet()
    //{
    //    mypool.CreateBullet();
    //}





}
