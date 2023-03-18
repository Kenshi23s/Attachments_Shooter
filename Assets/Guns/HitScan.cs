using System;
using UnityEngine;
using static GunStats;

public class HitScan
{
    GunStats stats;
    Camera cam;
    Transform myShootPos;
    float range;

    public HitScan(Camera cam, Transform shootPos, float range)
    {
        this.cam = cam;
        myShootPos = shootPos;
        this.range = range;
    }

    //Action <RaycastHit> OnHit;
    public bool HitSomething(out RaycastHit hit)
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, stats.myGunStats[StatNames.Range]))
        {
            Vector3 dir = hit.point - myShootPos.position;
            if (Physics.Raycast(myShootPos.transform.position, dir, out hit, range))
            {
               
                return true;
            }
           
        }
        return false;
    }


    
}
