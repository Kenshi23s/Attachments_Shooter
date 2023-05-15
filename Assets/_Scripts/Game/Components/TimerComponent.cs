using System;
using UnityEngine;

public class TimerComponent : MonoBehaviour
{
    float _initialTime;
    float _currentTime;

    public float timeLeft => _currentTime;

    public event Action OnFinish,OnStart;
    public event Action<float> whileTimerRunning;



    private void Awake() => enabled = false;

    public void SetTimer(float initialTime)
    {
        this._initialTime = initialTime;
        enabled= true;     
        OnStart?.Invoke();
    }

    public void RestartTimer() => SetTimer(_initialTime);

    public void AddTime(float timeToAdd) => _currentTime+= 1 * timeToAdd;

    public void SubstractTime(float timeToAdd) => _currentTime -= 1 * timeToAdd;

    private void Update()
    {
        _currentTime-=Time.deltaTime;
        whileTimerRunning?.Invoke(_currentTime);
        if (_currentTime < 0) 
        {
            OnFinish?.Invoke(); 
            _currentTime = 0; 
            enabled = false;
        }
       
    }

}
