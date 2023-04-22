using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
   
    
 

    public override bool WasKilled()
    {
        return lifeHandler.life<=0;

    }

   

   
}
