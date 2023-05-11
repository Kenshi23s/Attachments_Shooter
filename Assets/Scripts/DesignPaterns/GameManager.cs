using System;
using UnityEngine;
[RequireComponent(typeof(FloatingTextManager))]
[RequireComponent(typeof(TickEventsManager))]
public class GameManager : MonoSingleton<GameManager>
{
    [NonSerialized]public FloatingTextManager floatingTextManager;
    public ParticlePool _vfxPool;

    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();     
        _vfxPool = new ParticlePool();   
        
    }

}
