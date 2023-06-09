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

        if (_worm.anim.isOptimizable)
        {
            Debug.Log("andru no sabe riggear");
        }
    } 

    public override void OnUpdate()
    {
            
    }
}
