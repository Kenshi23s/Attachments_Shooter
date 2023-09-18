using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProceduralPlatform;

public class GadgetWall : Gadget
{
    public Transform shootPosition;
    public SolidCreator_Projectile sample;

    public float cooldownShoot;
    public float shootForce;
    public bool CanShoot = true;

    [field: SerializeField] public PlatformsParameters PlatformParameters { get; private set; }


    public override bool UseGadget(IGadgetOwner owner)
    {
        if (!CanShoot) return false;

        //instancio y posiciono proyectil
        var x = Instantiate(sample,shootPosition.position,Quaternion.identity);
        x.transform.forward = shootPosition.forward;
        //seteo los dueños de la bala
        FList<GameObject> owners = FList.Create(gameObject) + owner.OwnerGameObject;
        x.LaunchProjectile(shootForce * shootPosition.forward, owners, PlatformParameters);

        return true;
    }

    public IEnumerator CooldownStart()
    {
        CanShoot = true;
        yield return new WaitForSeconds(cooldownShoot);
        CanShoot = false;
    }

    public override void AwakeContinue() { }
    
}
