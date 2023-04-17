using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    public override int TakeDamage(int dmgDealt)
    {
        lifeHandler.Damage(dmgDealt);
        return dmgDealt;
    }

    public override bool WasKilled()
    {
        if (lifeHandler.life < 0)             
            return true;

        return false;

    }

   

   
}
