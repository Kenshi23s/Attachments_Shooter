using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_Shoot_Acid : Worm_State<Worm_AttackState>
{
    float _animationDuration, _timeSinceEnter;
    public Worm_State_Shoot_Acid(Enemy_Worm worm) : base(worm)
    {
        _animationDuration = _worm.AnimationLengths["ShootAcid"];
    }


    public override void OnEnter()
    {      
        _worm.anim.SetTrigger("ShootAcid");
        _worm.anim.SetFloat("Speed", 0);
    }

    public override void OnUpdate()
    {
        _timeSinceEnter += Time.deltaTime;
        if (_timeSinceEnter >= _animationDuration)
        {
            _fsm.ChangeState(Worm_AttackState.Flank);
            return;
        }

        Vector3 dir = Player_Movement.position - _worm.transform.position;
        _worm.transform.forward = new Vector3(dir.x, 0, dir.z);
    }
  

    public override void OnExit() => _worm.anim.SetFloat("Speed", 0);
    
}
