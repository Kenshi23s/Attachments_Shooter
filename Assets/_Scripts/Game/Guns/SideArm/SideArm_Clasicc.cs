using UnityEngine;
[RequireComponent(typeof(BulletHandler))]
public class SideArm_Clasicc : AutomaticGun
{
    BulletHandler b_Handler;
    protected override void OptionalInitialize()
    {
        base.OptionalInitialize();
        b_Handler = GetComponent<BulletHandler>();
    }


    public override void Shoot()
    {
        _debug.Log($"Shot fired by{gameObject.name}");
         BaseBulltet bullet = b_Handler.GetBullet();
        if (bullet!=null)        
            bullet.SetGunAndDispatch(this, OnHitCallBack);
        
        else        
            Debug.LogError("Error, bala == null, la pool no esta integrada o la bala default cambio");
                
    }

    public override bool ShootCondition()
    {
        // aca chequearia las condiciones para disparar
        return _rateFireHandler.canShoot;
    }
}
