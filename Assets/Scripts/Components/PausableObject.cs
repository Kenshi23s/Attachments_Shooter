using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//componente q se usa para pausar objetos
public class PausableObject : MonoBehaviour, IPausable
{
    public event Action onPause;
    public event Action onResume;

    private void Awake()
    {
        enabled = false;
        ScreenManager.AddPausable(this);
    }
 
    public void Pause() => onPause?.Invoke();

    public void Resume() => onResume?.Invoke();
    
}
