using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//componente q se usa para pausar objetos
public class PausableObject : MonoBehaviour, IPausable
{
    public event Action OnPause;
    public event Action OnResume;

    private void Awake()
    {
        enabled = false;
        ScreenManager.AddPausable(this);
    }
 
    public void Pause() => OnPause?.Invoke();

    public void Resume() => OnResume?.Invoke();
    
}
