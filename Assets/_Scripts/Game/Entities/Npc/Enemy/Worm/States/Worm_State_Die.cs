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
        _worm.StartCoroutine(Coroutine());
    } 

    IEnumerator Coroutine()
    {
        _worm.health.canTakeDamage = false;
        IEnumerable<Collider> col = FList.Create(_worm.GetComponent<Collider>()) + _worm.GetComponentsInChildren<Collider>();
        yield return null;
        foreach (var item in col) item.enabled = false;
    }

    
}
