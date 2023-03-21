using System;
using FacundoColomboMethods;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GunManager : MonoBehaviour
{
    [SerializeField]List<GunFather> myGuns = new List<GunFather>();
    [SerializeField]GunFather _actualGun;
     public GunFather actualGun=> _actualGun;

    public static GunManager instance;

    event Action<HitData> onActualGunHit;
    
    event Action OnSwapWeapons;

    //awake para inicializacion
    void Awake()
    {
        instance = this;
        //myGuns = ColomboMethods.GetChildrenComponents<GunFather>(this.transform).ToList();
    }

    //start para comunicacion
    void Start()
    {
        if (actualGun == null)
        {
            _actualGun = myGuns[UnityEngine.Random.Range(0, myGuns.Count)];
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
