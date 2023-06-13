using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_Worm;

public class Worm_State_Attack : Worm_State<EWormStates>
{
    public Worm_State_Attack(Enemy_Worm worm) : base(worm)
    {
        _attackFSM = new StateMachine<Worm_AttackState>();
        _attackFSM.Initialize(worm._debug);

        _attackFSM.CreateState(Worm_AttackState.Pursuit,   new Worm_State_Pursuit(worm));
        _attackFSM.CreateState(Worm_AttackState.Melee,     new Worm_State_Melee(worm));
        _attackFSM.CreateState(Worm_AttackState.ShootAcid, new Worm_State_Shoot_Acid(worm));
        _attackFSM.CreateState(Worm_AttackState.GrabDirt,  new Worm_State_Grab_Dirt(worm));
        _attackFSM.CreateState(Worm_AttackState.ShootDirt, new Worm_State_Shoot_Dirt(worm));
        _attackFSM.CreateState(Worm_AttackState.Flank,     new Worm_State_Flank(worm));
    }

    public enum Worm_AttackState
    {
        Pursuit,
        Melee,
        ShootAcid,
        GrabDirt,
        ShootDirt,
        Flank
    }

    StateMachine<Worm_AttackState> _attackFSM;

    public override void OnEnter()
    {
        _worm.anim.SetBool("Attacking", true);
        MakeDecision();
    }

    void MakeDecision()
    {
        // NOTA: Esto se podria optimizar si se sabe que rango es mas chico. En ese caso primero se chequearia por 
        // el rango mas chico, y recien despues por los mas grandes.
        bool inAcidRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.ShootAcidRadius);
        bool inDirtRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.ShootDirtRadius);
        bool inMeleeRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.MeleeAttackRadius);

        Worm_AttackState key =
          inMeleeRange ? Worm_AttackState.Melee
        : inDirtRange ? Worm_AttackState.GrabDirt
        : inAcidRange ? Worm_AttackState.ShootAcid
        : Worm_AttackState.Pursuit;

        _attackFSM.ChangeState(key);
    }



    public override void OnUpdate()
    {
        // Si se perdio de vista al jugador, pasar al idle
        if (!_worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.LoseSightRadius))
        {
            _worm.fsm.ChangeState(EWormStates.Idle);
            return;
        }

        _attackFSM.Execute();
    }

    public override void OnExit() 
    {
        _attackFSM.AnulateStates();
        _worm.anim.SetBool("Attacking", false);
    } 

    public override void GizmoShow() => _attackFSM.StateGizmos();
    

}
