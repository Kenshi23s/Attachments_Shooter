using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingTally : Perk
{
    int damageAdded;
    [SerializeField,Range(0,2)]
    int _dmgScaling;
    int timesApplied;
    


    internal override void InitializePerk(GunFather gun)
    {
       myGun = gun;
       timesApplied = 0;

       myGun.OnHit += AddDamage;

       myGun.OnReload += ResetDamage;
       myGun.OnStow += ResetDamage;
    }

    private void ResetDamage()
    {
        if (timesApplied >= 1)
        {
            myGun.damageManager.DecraseDamage(damageAdded);
            timesApplied = 0;
        }
    }

    private void AddDamage(HitData data)
    {
        if (timesApplied >= 1 && data.Target.WasKilled()==true)
        {
            myGun.damageManager.IncreaseDamage(damageAdded);
            
        }
    }



}
