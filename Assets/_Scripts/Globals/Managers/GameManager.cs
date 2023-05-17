using System;
using UnityEngine;
[RequireComponent(typeof(FloatingTextManager))]
[RequireComponent(typeof(TickEventsManager))]
[RequireComponent(typeof(AudioPool))]
public class GameManager : MonoSingleton<GameManager>
{
    [NonSerialized]public FloatingTextManager floatingTextManager;
    [NonSerialized]
    public ParticlePool vfxPool;

    protected override void SingletonAwake()
    {
        base.SingletonAwake();     
        vfxPool = new ParticlePool();   
        
    }

}
