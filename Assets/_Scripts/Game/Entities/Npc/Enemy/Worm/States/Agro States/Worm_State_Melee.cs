using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_Melee : Worm_State<Worm_AttackState>
{
    float _animationDuration, _timeSinceEnter;
    public Worm_State_Melee(Enemy_Worm worm) : base(worm) 
    {
        _animationDuration = _worm.AnimationLengths["Melee"];
    }

    public override void OnEnter()
    {
        _worm.anim.SetTrigger("Melee");
        _worm.StartCoroutine(_worm.SpawnMeleeHitbox());
        _worm.OnStun += _worm.CancelMelee;
        _worm.health.OnKilled += _worm.CancelMelee;
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
        _worm.OnStun -= _worm.CancelMelee;
        _worm.health.OnKilled -= _worm.CancelMelee;
    }

    public override void GizmoShow()
    {
        Gizmos.DrawWireSphere(_worm.transform.position, _worm.MeleeAttackRadius);     
    }
}
