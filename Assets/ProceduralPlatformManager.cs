using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPlatformManager : MonoSingleton<ProceduralPlatformManager>
{
    public Dictionary<ProceduralPlatform, List<ProceduralPlatform>> platforms = new();

    public PoolObject<ProceduralPlatform> pool { get; private set; } = new();
    public ProceduralPlatform sample;

    protected override void SingletonAwake()
    {

        Action<ProceduralPlatform> turnOn = x => x.gameObject.SetActive(true);

        Action<ProceduralPlatform> turnOff = x =>
        {
            x.gameObject.SetActive(false);
            x.CleanObject();
        };

        Func<ProceduralPlatform> build = () => Instantiate(sample);

        pool.Intialize(turnOn,turnOff,build);
    }



    public void AddNode(ProceduralPlatform owner,ProceduralPlatform NewNeighbor)
    {
        if (!platforms.ContainsKey(owner))       
            platforms.Add(owner, new List<ProceduralPlatform>());
        
        platforms[owner].Add(NewNeighbor);
    }

    public List<ProceduralPlatform> GetNeighbors(ProceduralPlatform owner)
    {
        if (platforms.ContainsKey(owner))     
           return platforms[owner];
        
        return new List<ProceduralPlatform>();
    }



   

   
}
