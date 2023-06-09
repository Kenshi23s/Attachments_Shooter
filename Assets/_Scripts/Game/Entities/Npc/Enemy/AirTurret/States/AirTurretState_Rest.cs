using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_AirTurret;
public class AirTurretState_Rest : IState<AirTurretState>
{
    Enemy_AirTurret _turret;
    StateMachine<string> _fsm;

    public AirTurretState_Rest(Enemy_AirTurret _turret, StateMachine<string> _fsm)
    {
        this._turret = _turret;
        this._fsm = _fsm;
    }

    public void OnEnter() { }    
      
    public void OnUpdate()
    {
      if (_turret.AlignCanon(false)) _fsm.ChangeState("Idle");
    }

    public void OnExit() { }   

    public void GizmoShow() { }

    public void SetStateMachine(StateMachine<AirTurretState> fsm)
    {
        throw new System.NotImplementedException();
    }
}
