using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingTally : Perk
{
    int damageAdded;
    [SerializeField,Range(0,2)]
    int _dmgScaling;
    int timesApplied;
    


    internal override void InitializePerk(Gun gun)
    {
       myGun = gun;
       timesApplied = 0;

       myGun.onHit += AddDamage;

       myGun.onReload += ResetDamage;
       myGun.onStow += ResetDamage;
    }

    private void ResetDamage()
    {
        if (timesApplied >= 1)
        {
            myGun.damageHandler.DecraseDamage(damageAdded);
            timesApplied = 0;
        }
    }

    private void AddDamage(HitData data)
    {
        if (timesApplied >= 1 && data.dmgData.wasKilled==true)
        {
            myGun.damageHandler.IncreaseDamage(damageAdded);
            
        }
    }



}
