using System;
using FacundoColomboMethods;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Attachment;
[RequireComponent(typeof(AttachmentManager))]
public class GunHandler : MonoBehaviour
{
    [SerializeField]List<Gun> myGuns = new List<Gun>();
    [SerializeField]Gun _actualGun;
    public Gun actualGun => _actualGun;
    public Player_Handler myPlayer { get; private set; }
   
    event Action<HitData> onActualGunHit;

    int actualGunCount;

    //event Action OnSwapWeapons;

    //awake para inicializacion

    //Esto esta mal, rompe con solid, preguntarle a jocha como hacerlo de mejor manera, pero por ahora...
    static Transform _sightPoint;
    public static Vector3 sightPosition=>_sightPoint.position; 

    public void SetPlayer(Player_Handler player) => myPlayer = player;
  
    
 
    private void Awake() => myGuns = ColomboMethods.GetChildrenComponents<Gun>(this.transform).ToList();
   
    //start para comunicacion
    void Start()
    {
        if (actualGun == null) _actualGun = myGuns[UnityEngine.Random.Range(0, myGuns.Count)];

        
        Debug.LogWarning("Armas en full auto, asegurarse q tengan el RateofFire != 0 ");
    }


    void OnSightChange()
    {
        AttachmentType key = AttachmentType.Sight;
        AttachmentHandler x = _actualGun.attachmentHandler;
        x.AddOnChangeEvent(key, () =>
        {
             var y = x.activeAttachments[key].GetComponent<Sight>();
              
            _sightPoint = y != null ? y.sightPoint : null;
           
        });
    }

    void Update()
    {
        foreach (Gun item in myGuns) item.GunUpdate();

        if (ScreenManager.IsPaused()) return;
      
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TriggerActualGun();
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            ReleaseTriggerActualGun();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            actualGunCount = actualGunCount >= myGuns.Count ? actualGunCount++:0;
            SwapWeapons(myGuns[actualGunCount]);
        }

       
    }

    
    public void TriggerActualGun() => actualGun.PressTrigger();
    public void ReleaseTriggerActualGun() => actualGun.ReleaseTrigger();

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
