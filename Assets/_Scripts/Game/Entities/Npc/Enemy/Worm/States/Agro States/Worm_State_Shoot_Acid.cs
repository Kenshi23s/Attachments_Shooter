using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm_State_Shoot_Acid : Worm_State
{
    public Worm_State_Shoot_Acid(Enemy_Worm worm) : base(worm)
    {
       
    }  

    public override void OnEnter()
    {      
        _worm.anim.SetTrigger("ShootAcid");
        _worm.anim.SetFloat("Speed", 0);
    }

    public override void OnUpdate() => _worm.transform.forward = Player_Movement.position - _worm.transform.position;
  

    public override void OnExit() => _worm.anim.SetFloat("Speed", 0);
    
}
