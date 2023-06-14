using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Worm_State_Attack;

public class Worm_State_Shoot_Dirt : Worm_State<Worm_AttackState>
{
    float _animationDuration, _timeSinceEnter;

    public Worm_State_Shoot_Dirt(Enemy_Worm worm) : base(worm) 
    {
        _animationDuration = _worm.AnimationLengths["ShootDirt"];
    }

    public override void OnEnter()
    {
        _timeSinceEnter = 0;
        _worm.anim.SetTrigger("ShootDirt");
        _worm.StartCoroutine(_worm.ShootDirt());
    }

    public override void OnUpdate()
    {
        _timeSinceEnter += Time.deltaTime;
        if (_timeSinceEnter >= _animationDuration)
        {
            _fsm.ChangeState(Worm_AttackState.Flank);
            return;
        }
    }

    public override void OnExit()
    {
        
    }

}
