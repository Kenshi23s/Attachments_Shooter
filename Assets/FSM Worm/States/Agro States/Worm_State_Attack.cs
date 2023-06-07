using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm_State_Attack : Worm_State
{
    public Worm_State_Attack(Enemy_Worm worm) : base(worm)
    {
        _attackFSM = new StateMachine<Worm_AttackState>();

    }

    public enum Worm_AttackState
    {
        Pursuit,
        Melee,
        ShootAcid,
        AbsorbDirt,
        ShootDirt
    }

    StateMachine<Worm_AttackState> _attackFSM;

   
    public override void OnUpdate()
    {
        _attackFSM.Execute();
    }


    public override void OnExit()
    {
        _attackFSM.AnulateStates();
    }


}
