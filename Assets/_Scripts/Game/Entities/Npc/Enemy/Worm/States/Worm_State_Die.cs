using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Enemy_Worm;

public class Worm_State_Die : Worm_State<EWormStates>
{
    public Worm_State_Die(Enemy_Worm worm) : base(worm)
    {
    }
 

    public override void OnEnter()
    {
        _worm.AI_move.CancelMovement();
        _worm.anim.SetTrigger("Die");
        _worm.CanBeStunned = false;

        _worm.health.canTakeDamage = false;
        _worm.AI_move.Movement._rb.isKinematic = true;
        foreach (var col in _worm.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

    }


    
}
