using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public FloatingTextManager floatingTextManager;

    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();
        floatingTextManager.Initialize(transform);

    }
}
