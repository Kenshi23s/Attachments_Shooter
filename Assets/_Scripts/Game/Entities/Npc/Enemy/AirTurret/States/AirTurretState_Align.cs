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
            bool seeing = ColomboMethods.InLineOffSight(_myTurret.turretEYE.position, _myTurret.target.position,
               _myTurret.wallMask);     
            if (seeing)
            {
                if (_myTurret.AlignBase(_myTurret.target.position) && _myTurret.AlignCanon(true))
                {
                    _turretFsm.ChangeState("Shoot");
                }
            }                 
            return;
        }
        _turretFsm.ChangeState("Rest");
    }

    public void GizmoShow()
    {
        if (_myTurret.target == null) return;
               
        Vector3 dir = _myTurret.target.position - _myTurret.turretEYE.position;
        RaycastHit hit;

        if (Physics.Raycast(_myTurret.turretEYE.position, dir, out hit, dir.magnitude, LayerMask.NameToLayer("Wall")))
        {
            Debug.Log("Gizmo");
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_myTurret.turretEYE.position, hit.point);
            Gizmos.DrawWireSphere(hit.point,10f );
        }
    } 

    public void OnEnter() { }

    public void OnExit() { }
   
}
