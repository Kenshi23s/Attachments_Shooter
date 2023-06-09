using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm_State_Shoot_Dirt : Worm_State
{
    public Worm_State_Shoot_Dirt(Enemy_Worm worm) : base(worm) {}

    public override void OnEnter()
    {
        _worm.anim.SetTrigger("ShootDirt");
        _worm.anim.SetFloat("Speed", 0);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        
    }

}
