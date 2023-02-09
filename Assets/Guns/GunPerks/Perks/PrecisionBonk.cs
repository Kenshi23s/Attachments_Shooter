using UnityEngine;

public class PrecisionBonk : Perk
{
    float precisionHits;
    float maxPrecisionHits;
    float damageAux;
    bool damageApplied;

    internal override void InitializePerk(GunFather gun)
    {
        myGun = gun;

        myGun.OnPrecisionHit += AddPoint;
        myGun.OnShoot += CheckBullets;

        myGun.OnReload += ResetPoints;
        myGun.OnStow += ResetPoints;

        precisionHits = 1;

    }

    void AddPoint() => precisionHits = Mathf.Clamp(precisionHits++ , 1, maxPrecisionHits);

    void ResetPoints()
    {
        
        if (damageApplied)
        {
            myGun.AddDamage(-myGun._actualDamage * precisionHits);
            damageApplied= false;
        }
        precisionHits = 1;
    } 

    void CheckBullets()
    {
        if (myGun._actualAmmo <= 1)
        {

            myGun.AddDamage(myGun._actualDamage * precisionHits);
            damageApplied = true;
        }
    }
}
