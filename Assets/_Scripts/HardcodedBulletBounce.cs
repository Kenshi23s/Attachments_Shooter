using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodedBulletBounce : MonoBehaviour
{
    Attachment attachment;
    RaycastComponent raycastComponent;

    private void Awake()
    {
        attachment.owner.GetComponent<RaycastComponent>();
    }

    void Bounce() 
    {

        //raycastComponent.ShootRaycast();
    }
}
