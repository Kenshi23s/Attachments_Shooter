using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoSingleton<Bullet_Manager>
{
    BulletPool mypool;

    // Start is called before the first frame update
    protected override void ArtificialAwake()
    {
        mypool = new BulletPool();    
        mypool.Initialize();
        
    }

     public BaseBulltet GetBullet(int key) => mypool.GetBullet(key);


    public int CreateBulletPool(BaseBulltet bullet) => mypool.CreateBullet(bullet);








}
