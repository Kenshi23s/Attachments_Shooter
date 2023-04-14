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

    //public abstract bool WasCrit();

    public override bool WasKilled()
    {
        //if (lifeHandler.life<0)
        //{
        //    OnDeath?.Invoke();
        //    return true;
        //} 
            

        return false;
        
    }

    public override void Pause() 
    {

       
    }
   
    public override void Resume() { }

   
}
