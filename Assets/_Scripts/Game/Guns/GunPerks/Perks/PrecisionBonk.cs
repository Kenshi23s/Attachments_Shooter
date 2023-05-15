using UnityEngine;

public class PrecisionBonk : Perk
{
    int precisionHits;
    int maxPrecisionHits;
    int damageAux;
    bool damageApplied;

    internal override void InitializePerk(Gun gun)
    {
        myGun = gun;

        myGun.onHit += AddPoint;
        myGun.onShoot += CheckBullets;

        myGun.onReload += ResetPoints;
        myGun.onStow += ResetPoints;

        precisionHits = 1;

    }

    void AddPoint(HitData data) => precisionHits =
    data.dmgData.wasCrit ? Mathf.Clamp(precisionHits++, 1, maxPrecisionHits) : precisionHits;
    //Mathf.Clamp(precisionHits++ , 1, maxPrecisionHits)

    void ResetPoints()
    {
        
        if (damageApplied)
        {
            myGun.damageHandler.DecraseDamage((int)(-myGun.damageHandler.actualDamage * precisionHits));
            damageApplied= false;
        }
        precisionHits = 1;
    } 

    void CheckBullets()
    {
        if (myGun._actualAmmo == 1)
        {

            myGun.damageHandler.IncreaseDamage((myGun.damageHandler.actualDamage * precisionHits));
            damageApplied = true;
        }
    }
}
