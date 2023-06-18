 using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(TickEventsManager))]
public class GameManager : MonoSingleton<GameManager>
{
    [NonSerialized]
    public ParticlePool vfxPool;

    public LineRenderer sampleLineSign;

    protected override void SingletonAwake()
    {
           
        vfxPool = new ParticlePool();   
        
    }
}
