using UnityEngine;

public class PrecisionBonk : Perk
{
    int precisionHits;
    int maxPrecisionHits;
    int damageAux;
    bool damageApplied;

    internal override void InitializePerk(GunFather gun)
    {
        myGun = gun;

        myGun.OnHit += AddPoint;
        myGun.OnShoot += CheckBullets;

        myGun.OnReload += ResetPoints;
        myGun.OnStow += ResetPoints;

        precisionHits = 1;

    }

    void AddPoint(HitData data) => precisionHits = data.Target.WasCrit() ? Mathf.Clamp(precisionHits++, 1, maxPrecisionHits) 
                                                                         : precisionHits;
    //Mathf.Clamp(precisionHits++ , 1, maxPrecisionHits)

    void ResetPoints()
    {
        
        if (damageApplied)
        {
            myGun.AddDamage((int)(-myGun._actualDamage * precisionHits));
            damageApplied= false;
        }
        precisionHits = 1;
    } 

    void CheckBullets()
    {
        if (myGun._actualAmmo <= 1)
        {

            myGun.AddDamage((myGun._actualDamage * precisionHits));
            damageApplied = true;
        }
    }
}
