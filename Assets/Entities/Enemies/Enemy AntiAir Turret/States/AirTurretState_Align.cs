using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTurretState_Align : IState
{
    Enemy_AirTurret _myTurret;
    StateMachine<string> _turretFsm;

    public AirTurretState_Align(Enemy_AirTurret myTurret, StateMachine<string> turretFsm)
    {
        _myTurret = myTurret;
        _turretFsm = turretFsm;
    }

    public void OnUpdate()
    {
        //si el blanco no es null    
        if (_myTurret.target != null)
        {
            bool seeing = ColomboMethods.InLineOffSight(_myTurret.transform.position, _myTurret.transform.position,3);
            //mi base esta alineada                 // y mi ca�on tambien
            if (_myTurret.AlignBase(_myTurret.target.position) && _myTurret.AlignCanon(true))            
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
