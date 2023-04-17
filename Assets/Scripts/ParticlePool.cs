using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool
{
   
    Dictionary<int,PoolObject<GameObject>> _vfxPools = new Dictionary<int, PoolObject<GameObject>>();
   //devuelve la key a la pool
    public int CreateVFXPool(GameObject particle)
    {
        PoolObject<GameObject> pool = new PoolObject<GameObject>();

        Action<GameObject> turnOn = (x) => { x.gameObject.SetActive(true); };
        Action<GameObject> turnOff = (x) => { x.gameObject.SetActive(false); };
        Func<GameObject>  build = () => 
        {
            GameObject particleClone = GameObject.Instantiate(particle);
            return particleClone;

        };

        int key = particle.GetHashCode();

        pool.Intialize(turnOn,turnOff,build);
        _vfxPools.Add(key, pool);

        return key;
           
    }

    public GameObject GetVFX(int key) => _vfxPools.ContainsKey(key) ? _vfxPools[key].Get() : null;





}
