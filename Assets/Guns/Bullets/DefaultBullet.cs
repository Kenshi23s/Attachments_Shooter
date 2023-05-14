using UnityEngine;
[RequireComponent(typeof(BulletMovement_Default))]
public class DefaultBullet : BaseBulltet
{
    protected override void OnHitEffect(HitData hit) { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        string _debugMsg = $"choque contra {other.gameObject.name} ";
        if (other.TryGetComponent(out IDamagable x))
        {
            Hit(x);
            _debugMsg += "y le hice daño";
        }
        _debug.Log(_debugMsg);

        
        //me desuscribo de el evento y borro la referencia al callback
      
    }
}
