using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile_Rock : MonoBehaviour,IDamagable
{
    int life = 100;
    Action onExplosion;
    float radius;
    GameObject owner;

    public void AddDamageOverTime(int TotalDamageToDeal, float TimeAmount) => TakeDamage(TotalDamageToDeal);

    public void AddKnockBack(Vector3 dir, float force) { }


    public DamageData TakeDamage(int dmgToDeal)
    {
        life -= dmgToDeal;
        Action death = delegate { };
        DamageData x = new DamageData();
        x.damageDealt = dmgToDeal;
        x.victim = this;
        x.wasCrit = false;
        
        if (life<=0) StartCoroutine(DestroyCoroutine());

        return x;
    }

    IEnumerator DestroyCoroutine()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return null;
        }
        Destroy(gameObject);

    }
    public DamageData TakeDamage(int dmgToDeal, Vector3 hitPoint)
    {
        return TakeDamage(dmgToDeal);
    }

    void Iniitialize(GameObject owner,float radius)
    {
        this.owner = owner;
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }


}
