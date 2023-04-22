using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    public override int TakeDamage(int dmgDealt)
    {
        return OnTakeDamage(dmgDealt);
    ;
    }

    public override bool WasKilled()
    {
        return true;

    }

   

   
}
