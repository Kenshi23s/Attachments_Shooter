using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public FloatingTextManager floatingTextManager;
    public ParticlePool _vfxPool;

    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();
        floatingTextManager.Initialize(transform);
        _vfxPool = new ParticlePool();

    }
}
