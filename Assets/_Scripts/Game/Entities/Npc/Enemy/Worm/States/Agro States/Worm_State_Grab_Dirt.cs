using FacundoColomboMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_Grab_Dirt : Worm_State<Worm_AttackState>
{
    float _animationDuration;
    float _timeSinceEnter;

    public Worm_State_Grab_Dirt(Enemy_Worm worm) : base(worm)
    {
        _animationDuration = _worm.AnimationLengths["GrabDirt"];
        
    }


    float dmgRadius;
    int dmgCount, requiredDmg4explosion = 3, explosionDamage = 25;
    int auxDmgResist;

    public override void OnEnter()
    {
        _worm.OnStun += _worm.CancelGrabDirt;

        _timeSinceEnter = 0;

        dmgCount = 0;
        _worm.health.OnTakeDamage += AddCount;
        auxDmgResist = _worm.health.dmgResist;
        _worm.health.dmgResist = 2;

        _worm.anim.SetTrigger("GrabDirt");
        _worm.StartCoroutine(_worm.GrabDirt());
    }


    public override void OnUpdate()
    {
        _timeSinceEnter += Time.deltaTime;
        if (_timeSinceEnter >= _animationDuration)
        {
            _fsm.ChangeState(Worm_AttackState.ShootDirt);
            return;
        }

        _worm.transform.forward = (Player_Movement.position - _worm.transform.position).normalized;
    }

    public override void OnExit()
    {
        _worm.OnStun -= _worm.CancelGrabDirt;
        _worm.health.OnTakeDamage -= AddCount;
        _worm.health.dmgResist = auxDmgResist;
    }

    void AddCount(int value)
    {
        dmgCount += value;
        if (dmgCount >= requiredDmg4explosion)
        {
            dmgCount = 0;
            DefensiveExplosion();
        }
    }

    void DefensiveExplosion()
    {
        IEnumerable<IDamagable> col = _worm.transform.position.GetItemsOFTypeAround<IDamagable>(dmgRadius).Where(x => x.GetType() != typeof(Enemy));
        foreach (var item in col) 
        { 
            item.TakeDamage(explosionDamage);
            Vector3 dir = item.Position() - _worm.transform.position;
            item.AddKnockBack(dir * _worm.DefenseKnockback);
        }
    }

    public override void GizmoShow()
    {
        Gizmos.DrawWireSphere(_worm.transform.position, dmgRadius);
    }
}
