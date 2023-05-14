using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//componente q se usa para pausar objetos

[DisallowMultipleComponent]
public class PausableObject : MonoBehaviour, IPausable
{
    public event Action onPause;
    public event Action onResume;

    private void Awake()
    {
        ScreenManager.AddPausable(this);
        enabled = false;
    }
 
    public void Pause() => onPause?.Invoke();

    public void Resume() => onResume?.Invoke();
    
}
