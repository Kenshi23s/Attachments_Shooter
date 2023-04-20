using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTurretState_Shoot : IState
{

    Enemy_AirTurret _turret;
    float _misileCD,_volleyCD;
    float _actualVolleyCD, actualMisileCD;
    float misilesPerVolley, misilesLeft;
    StateMachine<string> _turretFsm;


    Action _updateEvent;

    Transform misileTarget;

    public void OnEnter()
    {
        _actualVolleyCD = 0;

        actualMisileCD = 0;

        misileTarget = _turret.target;
        _updateEvent += ShootVolley;
    }   

    public void OnUpdate() => _updateEvent?.Invoke();

    public void OnExit()
    {
        _updateEvent= null;
    }
    public void GizmoShow()
    {
        throw new System.NotImplementedException();
    }


    void ShootVolley()
    {
        if (MisileCD())
        {
            _turret.ShootMisile(misileTarget);
            misilesLeft--;
            if (misilesLeft <= 0)
            {
                
                _updateEvent += VolleyCD;
            } 
        }
    }

    //tiempo entre misil y misil, se pregunta en un if
    bool MisileCD()
    {
        actualMisileCD -= Time.deltaTime;
        if (actualMisileCD <= 0)
        {
            actualMisileCD = _misileCD;
            return true;
        }
        return false;
    }

    void VolleyCD()
    {
        _actualVolleyCD -= Time.deltaTime;
        if (_turret.target == null) _turretFsm.ChangeState("Rest");      
          
        if (_actualVolleyCD > 0) return;

        _actualVolleyCD = _volleyCD;
        misileTarget = _turret.target;

        _updateEvent -= VolleyCD;
        _updateEvent += ShootVolley;


    }
}
