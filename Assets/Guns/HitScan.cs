using System;
using UnityEngine;
using static Attachment;
using static GunStats;

public class HitScan
{
    GunStats stats;
    Camera cam;
    Transform myShootPos;
    float range;
    GunFather myGun;
    Transform _shootPos;


    public HitScan(GunFather myGun)
    {
        this.myGun = myGun;
    }

    //Action <RaycastHit> OnHit;
    //public bool HitSomething(out RaycastHit hit)
    //{
    //    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, stats.myGunStats[StatNames.Range]))
    //    {
    //        Vector3 dir = hit.point - myShootPos.position;
    //        if (Physics.Raycast(myShootPos.transform.position, dir, out hit, range))
    //        {
               
    //            return true;
    //        }
           
    //    }
    //    return false;
    //}
    public bool Shoot(BaseBulltet bullet)
    {

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, stats.myGunStats[StatNames.Range]))
        {
           
            Vector3 dir = hit.point - _shootPos.position;
            if (Physics.Raycast(myShootPos.transform.position, dir, out hit, range))
            {
                // se llama a bullet manager para disparar la bala y se le pasa para donde debe ir
                return true;
            }

        }
        return false;
    }

    void GetShootPos()
    {       
        //es bastante largo o esta bien?
       _shootPos = myGun._myAttachMents.activeAttachments[AttachmentType.Muzzle].GetComponent<Muzzle>().shootPos;      
    }



}
