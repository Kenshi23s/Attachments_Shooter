using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_AirTurret;

public class AirTurretState_Align : IState<AirTurretState>
{
    Enemy_AirTurret _myTurret;
    StateMachine<AirTurretState> _turretFsm;

    public AirTurretState_Align(Enemy_AirTurret myTurret, StateMachine<AirTurretState> turretFsm)
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
                    _turretFsm.ChangeState(AirTurretState.SHOOT);
                }
            }                 
            return;
        }
        _turretFsm.ChangeState(AirTurretState.REST);
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

    public void SetStateMachine(StateMachine<AirTurretState> fsm)
    {
        throw new System.NotImplementedException();
    }
}
