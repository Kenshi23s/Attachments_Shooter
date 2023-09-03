using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProceduralPlatform;

public class GadgetWall : Gadget
{
    public Transform shootPosition;
    public SolidCreator_Projectile sample;

   [field: SerializeField] public PlatformsParameters PlatformParameters { get; private set; }

    public float cooldownShoot;
    public float shootForce;
    public bool CanShoot = true;

    public override bool UseGadget()
    {
        if (!CanShoot) return false;

        var x = Instantiate(sample,shootPosition.position,Quaternion.identity);
        x.transform.forward = shootPosition.forward;
        Debug.Log(GrabableComponent.Owner.gameObject);
        x.LaunchProjectile(shootForce * shootPosition.forward, FList.Create(gameObject) + GrabableComponent.Owner.gameObject ,PlatformParameters);

        return true;
    }

    public IEnumerator CooldownStart()
    {
        CanShoot = true;
        yield return new WaitForSeconds(cooldownShoot);
        CanShoot = false;
    }

    public override void AwakeContinue()
    {
        
    }
}
