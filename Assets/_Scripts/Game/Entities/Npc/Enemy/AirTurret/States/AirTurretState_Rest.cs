using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_AirTurret;
public class AirTurretState_Rest : IState<AirTurretState>
{
    Enemy_AirTurret _turret;
    StateMachine<AirTurretState> _fsm;

    public AirTurretState_Rest(Enemy_AirTurret _turret, StateMachine<AirTurretState> _fsm)
    {
        this._turret = _turret;
        this._fsm = _fsm;
    }

    public void OnEnter() { }    
      
    public void OnUpdate()
    {
      if (_turret.AlignCanon(false)) _fsm.ChangeState(AirTurretState.IDLE);
    }

    public void OnExit() { }   

    public void GizmoShow() { }

    public void SetStateMachine(StateMachine<AirTurretState> fsm)
    {
        
    }
}
