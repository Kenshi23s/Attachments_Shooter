using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FloatingTextManager))]
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
