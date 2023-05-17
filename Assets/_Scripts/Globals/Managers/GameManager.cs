using System;
using UnityEngine;
[RequireComponent(typeof(TickEventsManager))]
public class GameManager : MonoSingleton<GameManager>
{
    [NonSerialized]
    public ParticlePool vfxPool;

    protected override void SingletonAwake()
    {
        base.SingletonAwake();     
        vfxPool = new ParticlePool();   
        
    }

}
