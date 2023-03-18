using System;
using UnityEngine;

public class HitScan
{
    Camera cam;
    Transform myShootPos;
    float range;

    public HitScan(Camera cam, Transform shootPos, float range,Action<GunStats> OnStatsChange)
    {
        this.cam = cam;
        myShootPos = shootPos;
        this.range = range;
        OnStatsChange += (stats) => 
        {       
            range = stats.range;
        
        }; 
    }

    //Action <RaycastHit> OnHit;
    public bool HitSomething(out RaycastHit hit)
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
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
