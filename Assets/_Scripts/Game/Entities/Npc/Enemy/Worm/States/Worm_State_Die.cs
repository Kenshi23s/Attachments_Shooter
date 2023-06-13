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
        _worm.anim.SetTrigger("Die");
        IEnumerable<Collider> col = FList.Create(_worm.GetComponent<Collider>()) + _worm.GetComponentsInChildren<Collider>();
        _worm.health.canTakeDamage = false;
       
    } 

    public override void OnUpdate()
    {
            
    }
}
