using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTurretState_Idle : IState
{
    Enemy_AirTurret _turret;
    Transform _pivotBase;

    public AirTurretState_Idle(Enemy_AirTurret turret,Transform _pivotBase)
    {
        _turret = turret;
        this._pivotBase = _pivotBase;
    }

    public void OnUpdate()
    {
        Vector3 dir = _pivotBase.right;
        _turret.AlignBase(dir + _turret.transform.position);
       
    }

    public void GizmoShow() { }
   
    public void OnEnter() { }

    public void OnExit() { }
}
