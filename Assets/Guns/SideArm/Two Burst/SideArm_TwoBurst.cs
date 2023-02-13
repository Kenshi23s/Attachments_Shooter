using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideArm_TwoBurst : SideArm
{
    Transform _mainCam;

    protected override void OptionalInitialize()
    {
        base.OptionalInitialize();
        _mainCam = Camera.main.transform;
    }

    public override void GunShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(_mainCam.position, _mainCam.forward, out hit, _range))
        {
            //quien se encarga de decir si fue un critico, el arma o el enemigo?
            //bool IsCrit = false;
            var damagable = hit.transform.GetComponent<IDamagable>;

            if (damagable != null)
            {
                
                
            }
        }
      
    }

    

   
}
