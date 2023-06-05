using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//componente q se usa para pausar objetos con screenmanager(sin implementar por el momento)
[DisallowMultipleComponent]
public class PausableObject : MonoBehaviour, IPausable
{
    public event Action onPause;
    public event Action onResume;

    private void Awake()
    {
        ScreenManager.AddPausable(this);
        enabled = true;
    }

    private void OnDestroy()
    {
        ScreenManager.RemovePausable(this);
        onPause = default;
        onResume = default;
    }
    public void Pause() => onPause?.Invoke();

    public void Resume() => onResume?.Invoke();
    
}
