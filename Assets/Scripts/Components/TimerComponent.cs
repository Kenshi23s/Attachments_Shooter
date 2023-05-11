using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerComponent : MonoBehaviour
{
    float initialTime;
    float currentTime;

    public float timeLeft => currentTime;

    public event Action OnFinish,OnStart;
    public event Action<float> whileTimerRunning;



    private void Awake()
    {
        enabled= false;
    }

    void SetTimer(float InitialTime)
    {

    }
    private void Update()
    {
        
    }

}
