using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RaycastComponent))]
public class SideArm_Burst : Burst_Gun
{
    RaycastComponent _raycast;

    protected override void OptionalInitialize()
    {
        base.OptionalInitialize();
        _raycast= GetComponent<RaycastComponent>();
    }
    public override void ShootOnBurst()
    {
        Vector3 from = attachmentHandler.shootPos.position;
        Vector3 to = attachmentHandler.shootPos.forward;
        
       _raycast.ShootRaycast(from, OnHitCallBack);
    }

   
}
