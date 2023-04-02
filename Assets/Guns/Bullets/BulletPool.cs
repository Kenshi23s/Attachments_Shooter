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
   public BulletPool() {  }

    Dictionary<string, PoolObject<BaseBulltet>> _pools = new Dictionary<string, PoolObject<BaseBulltet>>();

    //para tener mas organizado la jerarquia, hago un GO que actua como carpeta
    GameObject poolGO;

    public void Initialize() => poolGO = GameObject.Instantiate(new GameObject("BulletPool"), Vector3.zero, Quaternion.identity);

    /// <summary>
    /// crea balas para la pool, requiere una key y un prefab de bullet
    /// (opcionalmente, se le puede pasar cuantas balas se hacen al principio)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="prefab"></param>
    /// <param name="prewarm"></param>
    public void CreateBullet(string key,BaseBulltet prefab, int prewarm = 5)
    {
        if (!_pools.ContainsKey(key))
        {
            GameObject father = GameObject.Instantiate(new GameObject($"bullet {key} storage"));
            father.transform.parent = poolGO.transform;
            Func<BaseBulltet> myBuild = () =>
            {

                BaseBulltet projectile = GameObject.Instantiate(prefab);
                projectile.transform.parent=father.transform;
                return projectile;
            };

            _pools.Add(key, new PoolObject<BaseBulltet>());

            _pools[key].Intialize(TurnOn, TurnOff, myBuild);
            return;
        }

        Debug.Log("ya existia esta bala, no creo nada");


    }
    /// <summary>
    /// devuelve una bala que tenga ese tipo de key, si no esta en la pool devuelve null
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public BaseBulltet AskForProjectile(string key)
    {
        if (_pools.ContainsKey(key))
        {
            BaseBulltet projectile = _pools[key].Get();
            projectile.Initialize(ReturnToPool, key);
            return projectile;
        }
        Debug.Log("no existe ese tipo de bala");
        return null;
    }

    void TurnOn(BaseBulltet x) => x.gameObject.SetActive(true);

    void TurnOff(BaseBulltet x) => x.gameObject.SetActive(false);
   
    void ReturnToPool(BaseBulltet value, string key) => _pools[key]?.Return(value);


}
