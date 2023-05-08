using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoSingleton<Bullet_Manager>
{
    BulletPool mypool;
    static BaseBulltet _defaultBulletPrefab;
    [SerializeField] BaseBulltet _defaultBulletSample;

    public static int defaultBulletKey => _defaultBulletKey;
    static int _defaultBulletKey = 0000;


    // Start is called before the first frame update
    protected override void ArtificialAwake()
    {
        mypool = new BulletPool();
        _defaultBulletPrefab = _defaultBulletSample;
        mypool.Initialize();
        _defaultBulletKey = mypool.CreateBullet(_defaultBulletPrefab);
    }

     public BaseBulltet GetProjectile(int key) 
        => mypool.AskForProjectile(key);

    public int CreateBulletPool(BaseBulltet bullet)
      => mypool.CreateBullet(bullet);

    //public BaseBulltet SetProjectile(string key, BulletProperties properties, Action<HitData> onHit)
    //    => mypool.AskForProjectile(key, properties, onHit);

    //public void CreateBullet()
    //{
    //    mypool.CreateBullet();
    //}





}
