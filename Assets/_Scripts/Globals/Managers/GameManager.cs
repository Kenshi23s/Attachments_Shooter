 using System;
using UnityEngine;
[RequireComponent(typeof(TickEventsManager))]
public class GameManager : MonoSingleton<GameManager>
{
    [NonSerialized]
    public ParticlePool vfxPool;

    public LineRenderer sampleLineSign;

    protected override void SingletonAwake()
    {
        base.SingletonAwake();     
        vfxPool = new ParticlePool();   
        
    }

}
