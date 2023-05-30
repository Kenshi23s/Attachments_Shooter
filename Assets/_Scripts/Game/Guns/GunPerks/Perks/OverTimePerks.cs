using System;
using UnityEngine;

public abstract class OverTimePerks : Perk
{

    public abstract override void InitializePerk(Gun gun);

    internal protected event Action<float> TimerUpdate;

    [Header("Timer")]
    [SerializeField] protected bool is_Active;

    [SerializeField] protected float _initialTime;

    [SerializeField] protected float _actualTime;




    private void LateUpdate() => TimerUpdate?.Invoke(Time.deltaTime);

    protected void RefreshTimer() => _actualTime = _initialTime;

    protected void AddTime(float timeToAdd) => _actualTime = Mathf.Clamp (_actualTime, 0, _initialTime);

}
