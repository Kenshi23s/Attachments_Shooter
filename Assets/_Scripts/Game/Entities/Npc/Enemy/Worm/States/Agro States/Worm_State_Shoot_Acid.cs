using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_Shoot_Acid : Worm_State<Worm_AttackState>
{
    public Worm_State_Shoot_Acid(Enemy_Worm worm) : base(worm)
    {
       
    } 
    

    public override void OnEnter()
    {      
        _worm.anim.SetTrigger("ShootAcid");
        _worm.anim.SetFloat("Speed", 0);
    }

    public override void OnUpdate()
    {
        Vector3 dir = Player_Movement.position - _worm.transform.position;
        _worm.transform.forward = new Vector3(dir.x, 0, dir.z);
    }
  

    public override void OnExit() => _worm.anim.SetFloat("Speed", 0);
    
}
