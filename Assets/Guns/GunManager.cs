using System;
using FacundoColomboMethods;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[RequireComponent(typeof(AttachmentManager))]
public class GunManager : MonoSingleton<GunManager>
{
    [SerializeField]List<GunFather> myGuns = new List<GunFather>();
    [SerializeField]GunFather _actualGun;
    public GunFather actualGun=> _actualGun;

   
    event Action<HitData> onActualGunHit;
    
    //event Action OnSwapWeapons;

    //awake para inicializacion
    
    
      
       
    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();
        myGuns = ColomboMethods.GetChildrenComponents<GunFather>(this.transform).ToList();
    }

    //start para comunicacion
    void Start()
    {
        if (actualGun == null)
        {
            _actualGun = myGuns[UnityEngine.Random.Range(0, myGuns.Count)];
        }
        Debug.LogWarning("Armas en full auto, asegurarse q tengan el RateofFire != 0 ");
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetKey(KeyCode.Mouse0))
        {
            TriggerActualGun();
        }
    }

    
    public void TriggerActualGun() => actualGun.Trigger();

    void SwapWeapons(GunFather actualWeapon)
    {
        if (!myGuns.Contains(actualWeapon)) AddGun(actualWeapon);

        foreach (GunFather gun in myGuns) if (actualGun != gun) gun.Stow();

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
