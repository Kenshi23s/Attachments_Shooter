using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm_State_Attack : Worm_State
{
    public Worm_State_Attack(Enemy_Worm worm) : base(worm)
    {
    }

    public enum Worm_AttackState
    {
        Pursuit,
        Melee,
        Shoot,
        Absorbdirt
    }

    StateMachine<Worm_AttackState> _AttackFSM;
    public override void OnUpdate()
    {
        _AttackFSM.Execute();
    }
}
