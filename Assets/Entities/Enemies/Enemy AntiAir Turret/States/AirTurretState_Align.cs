using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTurretState_Align : IState
{
    Enemy_AirTurret _myTurret;
    StateMachine<string> _turretFsm;

    public void OnUpdate()
    {
        //si el blanco no es null    
        if (_myTurret.target != null)
        {
            //mi base esta alineada                 // y mi cañon tambien
            if (_myTurret.AlignBase(_myTurret.target.position) && _myTurret.AlignCanon(Vector3.up))            
                _turretFsm.ChangeState("Shoot");            

            return;
        }
        _turretFsm.ChangeState("Rest");
    }

    public void GizmoShow()
    {
        
    }


    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

}
