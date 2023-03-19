using System;

using System.Collections.Generic;
using UnityEngine;


public class GunManager : MonoBehaviour
{
    List<GunFather> myGuns=new List<GunFather>();
    GunFather actualGun;

    public static GunManager instance;

    event Action onActualGunKill;
    event Action onActualGunHit;
    event Action onActualGunCritHit;
    event Action onActualGunCritKill;

    event Action OnSwapWeapons;

    void Awake()
    {
        instance = this;
        OnSwapWeapons += () =>
        {
          

        };
        
    }
    void Start()
    {
        if (actualGun == null)
        {
           actualGun = myGuns[UnityEngine.Random.Range(0, myGuns.Count)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void TriggerActualGun() => actualGun.Trigger();

    void SwapWeapons(GunFather actualWeapon)
    {
        if (!myGuns.Contains(actualWeapon))
        {
            AddGun(actualWeapon);
        }
        
        foreach (GunFather gun in myGuns)
        {
            if (actualGun != gun)
            {
               gun.Stow();
               
            }
         
        }
        actualGun.Draw();
    }

    bool AddGun(GunFather newGun)
    {
        if (!myGuns.Contains(newGun))
        {
            myGuns.Add(newGun);
            return true;
        }
        return false;
    }

    bool RemoveGun(GunFather newGun)
    {
        if (myGuns.Contains(newGun))
        {
            myGuns.Add(newGun);
            return true;
        }
        return false;
    }
}
