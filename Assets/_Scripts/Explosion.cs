using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FacundoColomboMethods;

public class Explosion : MonoBehaviour
{
    public float radius = 1f;
    public int minDamage = 2, maxDamage = 30;
    public float minKnockback = 0.2f, maxKnockback = 2f;

    [SerializeField] ParticleHolder _particleHolder;

    int vfxKey;
    private void Awake()
    {
        vfxKey = GameManager.instance.vfxPool.CreateVFXPool(_particleHolder);
    }

    // Start is called before the first frame update
    void Start()
    {
        var damageables = ColomboMethods.GetItemsOFTypeAround<IDamagable>(transform.position, radius);

        foreach (var damageable in damageables)
        {
            Vector3 vector = damageable.Position - transform.position;
            float t = vector.magnitude / radius;
            int damage = (int) Mathf.Lerp(maxDamage, minDamage, t);
            float knockback = Mathf.Lerp(maxKnockback, minKnockback, t);

            damageable.TakeDamage(damage);
            damageable.AddKnockBack(vector.normalized * knockback);
        }

        ParticleHolder particleHolder = GameManager.instance.vfxPool.GetVFX(vfxKey);
        particleHolder.transform.position = transform.position;
        particleHolder.transform.localScale *= radius;

        particleHolder.OnFinish += () => Destroy(gameObject);
    }

}
