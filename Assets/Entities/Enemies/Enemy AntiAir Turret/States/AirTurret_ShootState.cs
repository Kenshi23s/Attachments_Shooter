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

    event Action _updateEvent;

    Transform misileTarget;

    public AirTurretState_Shoot(Enemy_AirTurret _turret, float _misileCD, float _volleyCD, float _misilesPerVolley, StateMachine<string> _turretFsm)
    {
        this._turret = _turret;
        this._misileCD = _misileCD;
        this._volleyCD = _volleyCD;
        this.misilesPerVolley = _misilesPerVolley;
        this._turretFsm = _turretFsm;
    }

    public void OnEnter()
    {
        _actualVolleyCD = 0;
        actualMisileCD = 0;

        misilesLeft = misilesPerVolley;

        misileTarget = _turret.target;
        _updateEvent += ShootVolley;
    }   

    public void OnUpdate() => _updateEvent?.Invoke();

    public void OnExit() => _updateEvent = null;

    void ShootVolley()
    {
        if (MisileCD())
        {
            _turret.ShootMisile(misileTarget);
            misilesLeft--;
            Debug.Log(misilesLeft);
            if (misilesLeft <= 0)
            {
              
                _updateEvent -= ShootVolley;
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
        OnEnter();


    }

    public void GizmoShow()
    {

    }
}
