using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingTally : Perk
{
    float damageAdded;
    [SerializeField,Range(0,2)]
    float _dmgScaling;
    float timesApplied;
    


    internal override void InitializePerk(GunFather gun)
    {
       myGun = gun;
       timesApplied = 0;

       myGun.OnKill += AddDamage;

       myGun.OnReload += ResetDamage;
       myGun.OnStow += ResetDamage;
    }

    private void ResetDamage()
    {
        if (timesApplied >= 1)
        {
            myGun.SubstractDamage(damageAdded);
            timesApplied = 0;
        }
    }

    private void AddDamage()
    {
        if (timesApplied >= 1)
        {
            myGun.SubstractDamage(damageAdded);
            timesApplied = 0;
        }
    }



}
