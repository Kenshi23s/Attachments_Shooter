using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Charge_Gun : Gun
{
    bool isCharging;
    public bool IsCharging
    {
        get => isCharging;

        set
        {
            isCharging = enabled  = value;
        }
    }

    float _requiredTime;
    float _currentTime;
    float _checkTime;

    public float currentTime { get => _currentTime; set => _currentTime = Mathf.Clamp(_currentTime,0,_requiredTime); }

    float _currentCooldown;
    float _cooldownTime;

    // despues preguntar a jocha q le parece y como podria seguirlo
    public override void Trigger()
    {
       _checkTime = currentTime;
       if (!ShootCondition()) { IsCharging = false; return; }

        IsCharging = true;  currentTime += Time.deltaTime;
       
       if (currentTime >= _requiredTime)
       {
           Shoot(); CallOnShootEvent();
           _currentCooldown = _cooldownTime;
           IsCharging=false;
       }
    }
    private void LateUpdate()
    {
        if (_checkTime > currentTime)
        {
            currentTime -= Time.deltaTime;
            if (currentTime<=0) isCharging = false;
        }
    }

    public override bool ShootCondition() => _currentCooldown <= 0;
    
}
