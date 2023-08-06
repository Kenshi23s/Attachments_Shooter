using System;
using FacundoColomboMethods;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using static Attachment;
using UnityEngine.UI;

[RequireComponent(typeof(AttachmentManager))]
public class GunHandler : MonoBehaviour
{
    [SerializeField] List<Gun> myGuns = new List<Gun>();

    [field: SerializeField] public Gun ActualGun { get; private set; }
    public Player_Handler myPlayer { get; private set; }
   
    event Action<HitData> onActualGunHit;

    public event Action onActualGunShoot;
    int actualGunCount;
    [SerializeField] Image hitmarker;
    [SerializeField] RawImage crosshair;
    //event Action OnSwapWeapons;

    //awake para inicializacion

    //Transform _sightPoint;
    //public Vector3 SightPosition =>_sightPoint != null ? _sightPoint.localPosition : Vector3.zero; 
    public Vector3 SightPosition => ActualGun.attachmentHandler.aimPos.position; 

    public void SetPlayer(Player_Handler player) => myPlayer = player;

    [SerializeField] Gun DefaultGun;
    [SerializeField] Transform gunPos;
    private void Awake()
    {
        ActualGun = Instantiate(DefaultGun, gunPos);
        ActualGun.transform.position = gunPos.position;
        myGuns = transform.GetChildrenComponents<Gun>().ToList();
        if (ActualGun == null) ActualGun = myGuns[UnityEngine.Random.Range(0, myGuns.Count)];
        ActualGun.onShoot += onActualGunShoot;
        if (hitmarker!=null)
        {
            ActualGun.onHit += (x) => Hitmarker();
            hitmarker.color = hitmarker.color.SetAlpha(0);
        }

        ShakeCamera.OnHipPosReached += () => crosshair.gameObject.SetActive(true);
        ShakeCamera.OnAimPosReached += () => crosshair.gameObject.SetActive(false);
    }

    void Hitmarker()
    {
        StopCoroutine(HitmarkerUpdate());
        StartCoroutine(HitmarkerUpdate());

    }

    IEnumerator HitmarkerUpdate()
    {
        float currentAlpha = 1f;
        hitmarker.color = hitmarker.color.SetAlpha(currentAlpha);

        while (currentAlpha>0)
        {
            Debug.Log("hit");
            yield return null;
            currentAlpha-=Time.deltaTime;
            hitmarker.color = hitmarker.color.SetAlpha(currentAlpha);
        }
        hitmarker.color = hitmarker.color.SetAlpha(0);

    }
    //start para comunicacion
    void Start()
    {
       

        //SetupOnSightChange();
    
        Debug.LogWarning("Armas en full auto, asegurarse q tengan el RateofFire != 0 ");
    }


    //void SetupOnSightChange()
    //{
    //    AttachmentType key = AttachmentType.Sight;
    //    AttachmentHandler handler = _actualGun.attachmentHandler;
    //    handler.AddOnChangeEvent(key, () =>
    //    {
    //        if (handler.activeAttachments.TryGetValue(key,out var z))
    //        {
    //            var sight = handler.activeAttachments[key].GetComponent<Sight>();
    //            _sightPoint = sight.sightPoint;
    //            return;
    //        }
    //        _sightPoint = null;
    //    });
    //}

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

    
    public void TriggerActualGun() => ActualGun.PressTrigger();
    public void ReleaseTriggerActualGun() => ActualGun.ReleaseTrigger();

    void SwapWeapons(Gun actualWeapon)
    {
        if (!myGuns.Contains(actualWeapon)) AddGun(actualWeapon);

        foreach (Gun gun in myGuns) if (ActualGun != gun) gun.Stow();

        ActualGun.Draw();
      
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
