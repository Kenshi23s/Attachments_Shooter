using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ProceduralPlatformManager : MonoSingleton<ProceduralPlatformManager>
{
    public Dictionary<ProceduralPlatform, List<ProceduralPlatform>> platforms = new();

    public PoolObject<ProceduralPlatform> pool { get; private set; } = new();
    public ProceduralPlatform sample;
    [SerializeField] int _warmUp = 20;

    protected override void SingletonAwake()
    {
        Action<ProceduralPlatform> turnOn = x => x.gameObject.SetActive(true);

        Action<ProceduralPlatform> turnOff = y =>
        {
            y.gameObject.SetActive(false);
            y.CleanObject();
        };

        Func<ProceduralPlatform> build = () => Instantiate(sample);

        pool.Intialize(turnOn, turnOff, build, _warmUp);
    }

    
    /// <summary>
    /// guarda todos los nodos empezando desde start
    /// </summary>
    /// <param name="start"></param> 
    public async Task ChainStoreAll(ProceduralPlatform start)
    {
        if (!platforms.ContainsKey(start)) return;

        StorePlatform(start);
       
        //el task solo toma los argumentos en milisegundos
        await Task.Delay((int) (start._decayDelaySeconds * 1000));
        foreach (var item in GetNeighbors(start).Where(x => x.gameObject.activeInHierarchy == true))
        {
            await ChainStoreAll(item);      
        }
    }

    public void AddNode(ProceduralPlatform owner, ProceduralPlatform NewNeighbor)
    {
        if (!platforms.ContainsKey(owner))
            platforms.Add(owner, new List<ProceduralPlatform>());

        platforms[owner].Add(NewNeighbor);
    }

    public bool RemoveNode(ProceduralPlatform owner, ProceduralPlatform oldNeighbor)
    {
        if (!platforms.ContainsKey(owner)) return false;
        if (!platforms[owner].Contains(oldNeighbor)) return false;

        platforms[owner].Remove(oldNeighbor);
        return true;
    }

    public void RemoveOwner(ProceduralPlatform owner)
    {
        if (platforms.ContainsKey(owner))
            platforms.Remove(owner);

        foreach (var item in platforms.Select(x => x.Value).Where(x => x.Contains(owner)))
        {
            item.Remove(owner);
        }
    }

    public bool IsNeighbor(ProceduralPlatform owner, ProceduralPlatform x) 
    {
        if (platforms.ContainsKey(owner))        
            return platforms[owner].Contains(x);
        
        return false;
    
    }

    public List<ProceduralPlatform> GetNeighbors(ProceduralPlatform owner)
    {
        if (platforms.ContainsKey(owner))
            return platforms[owner];

        return new List<ProceduralPlatform>();
    }

    public void StorePlatform(ProceduralPlatform x) 
    {
        RemoveOwner(x);
        pool.Return(x);
    } 

}
