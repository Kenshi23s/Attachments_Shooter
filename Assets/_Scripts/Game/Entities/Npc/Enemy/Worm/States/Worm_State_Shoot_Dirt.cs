using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_Shoot_Dirt : Worm_State<Worm_AttackState>
{
    public Worm_State_Shoot_Dirt(Enemy_Worm worm) : base(worm) 
    {
        _worm.OnShootDirtAnimationFinished += () => _fsm.ChangeState(Worm_AttackState.Flank);
    }

    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        
    }

}
