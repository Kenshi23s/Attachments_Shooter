using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_Melee : Worm_State<Worm_AttackState>
{
    public Worm_State_Melee(Enemy_Worm worm) : base(worm) {}

    public override void OnEnter()
    {
        _worm.anim.SetTrigger("Melee");
    }

    public override void OnUpdate()
    {
        // Chequear cuando se termina la animacion de melee y pasar a flank
        
    }

    public override void OnExit()
    {

    }

    public override void GizmoShow()
    {
        Gizmos.DrawWireSphere(_worm.transform.position, _worm.MeleeAttackRadius);     
    }
}
