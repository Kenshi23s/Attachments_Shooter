using System;
using FacundoColomboMethods;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[RequireComponent(typeof(AttachmentManager))]
public class GunManager : MonoBehaviour
{
    [SerializeField]List<Gun> myGuns = new List<Gun>();
    [SerializeField]Gun _actualGun;
    public Gun actualGun => _actualGun;
    public Player_Handler myPlayer { get; private set; }
   
    event Action<HitData> onActualGunHit;
    
    //event Action OnSwapWeapons;

    //awake para inicializacion
    
    public void SetPlayer(Player_Handler player)
    {
        myPlayer = player;
    }
    
 
    private void Awake()
    {
        myGuns = ColomboMethods.GetChildrenComponents<Gun>(this.transform).ToList();
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

    int actualGunCount;
    void Update()
    {
      
        if (Input.GetKey(KeyCode.Mouse0))
        {
            TriggerActualGun();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            actualGunCount = actualGunCount >= myGuns.Count ? actualGunCount++:0;
            SwapWeapons(myGuns[actualGunCount]);
        }
    }

    
    public void TriggerActualGun() => actualGun.Trigger();

    void SwapWeapons(Gun actualWeapon)
    {
        if (!myGuns.Contains(actualWeapon)) AddGun(actualWeapon);

        foreach (Gun gun in myGuns) if (actualGun != gun) gun.Stow();

        actualGun.Draw();
    }

    bool AddGun(Gun newGun)
    {
        if (!myGuns.Contains(newGun))
        {
            myGuns.Add(newGun);
            return true;
        }
        return false;
    }


   
    bool RemoveGun(Gun newGun)
    {
        if (myGuns.Contains(newGun))
        {
            myGuns.Add(newGun);
            return true;
        }
        return false;
    }

   
}
