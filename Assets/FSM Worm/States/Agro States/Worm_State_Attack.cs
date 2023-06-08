using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_Worm;
public class Worm_State_Attack : Worm_State
{
    public Worm_State_Attack(Enemy_Worm worm) : base(worm)
    {
        _attackFSM = new StateMachine<Worm_AttackState>();
        _attackFSM.CreateState(Worm_AttackState.AbsorbDirt,new Worm_State_Reflect(worm));
        _attackFSM.CreateState(Worm_AttackState.Pursuit, new Worm_State_Pursuit(worm));
        _attackFSM.CreateState(Worm_AttackState.Melee, new Worm_State_Melee(worm));
        _attackFSM.CreateState(Worm_AttackState.ShootAcid, new Worm_State_Shoot(worm));
        _attackFSM.CreateState(Worm_AttackState.Flank, new Worm_State_Flank(worm));

    }

    public enum Worm_AttackState
    {
        Pursuit,
        Melee,
        ShootAcid,
        AbsorbDirt,
        ShootDirt,
        Flank
    }

    StateMachine<Worm_AttackState> _attackFSM;

    float _shootRange,_meleeRange;
    public override void OnEnter()
    {
        MakeDecision();
    }

  

    void MakeDecision()
    { 

        if (!_worm.AI_move.FOV.IN_FOV(Player_Movement.position))
        {
            _worm.fsm.ChangeState(EWormStates.Idle); return;
        }

        #region Select State

        Worm_AttackState  key = Worm_AttackState.Pursuit;
        float dist = Vector3.Distance(Player_Movement.position, _worm.transform.position);

        if (dist < _shootRange)
            key = Worm_AttackState.ShootAcid;

        if (dist < _meleeRange)
            key = Worm_AttackState.Melee;

        #endregion

        _attackFSM.ChangeState(key);
    }

    

    public override void OnUpdate()
    {
        _attackFSM.Execute();
    }


    public override void OnExit()
    {
       
        _attackFSM.AnulateStates();
    }


}
