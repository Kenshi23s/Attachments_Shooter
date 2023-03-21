using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
//public struct ProjectileData
//{
//    public string name;
//    public Projectile model; 
//    public ProjectileType type;
    
//}

public class BulletPool
{
   public BulletPool() { }

    Dictionary<string, PoolObject<BaseBulltet>> _pools = new Dictionary<string, PoolObject<BaseBulltet>>();
   

    public void CreateBullet(string key,BaseBulltet prefab, int prewarm = 5)
    {
        if (!_pools.ContainsKey(key))
        {
            Func<BaseBulltet> myBuild = () =>
            {
                BaseBulltet projectile = GameObject.Instantiate(prefab);
                return projectile;
            };

            _pools.Add(key, new PoolObject<BaseBulltet>());

            _pools[key].Intialize(TurnOn, TurnOff, myBuild);
            return;
        }

        Debug.Log("ya existia esta bala, no creo nada");


    }

    public BaseBulltet AskForProjectile(string key,BulletProperties properties,Action<HitData> onHit)
    {
        if (_pools.ContainsKey(key))
        {
            BaseBulltet projectile = _pools[key].Get();
            projectile.Initialize(ReturnToPool, key,properties,onHit);
            return projectile;
        }
        return null;
    }

    void TurnOn(BaseBulltet x) => x.gameObject.SetActive(true);

    void TurnOff(BaseBulltet x) => x.gameObject.SetActive(false);
   
    void ReturnToPool(BaseBulltet value, string key) => _pools[key]?.Return(value);


}
