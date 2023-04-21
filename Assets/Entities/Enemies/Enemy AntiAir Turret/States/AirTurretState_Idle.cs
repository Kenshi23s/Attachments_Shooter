using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTurretState_Idle : IState
{
    Enemy_AirTurret _turret;

    public AirTurretState_Idle(Enemy_AirTurret turret)
    {
        _turret = turret;
    }

    public void OnUpdate()
    {
        _turret.AlignBase(_turret.transform.right);
       
    }

    public void GizmoShow() { }
   
    public void OnEnter() { }

    public void OnExit() { }
}
