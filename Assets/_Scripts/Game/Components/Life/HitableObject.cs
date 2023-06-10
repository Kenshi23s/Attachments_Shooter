using System;
using UnityEngine;
//se usaria para hacer varios spots donde puede ser golpeado un enemigo
public class HitableObject : MonoBehaviour,IDamagable,IHealable
{
    LifeComponent _owner;

    [SerializeField,Tooltip("Marcar TRUE en caso de que sea punto critico")] 
    bool isCritSpot;
    [SerializeField, Range(0.1f,2f)] 
    float damageMultiplier;


    void Awake()
    {

        if (transform.root.TryGetComponent(out LifeComponent x))
        {
          
            SetOwner(x);
        }
        else
        {
            Debug.LogError("No hay padre con life component, me destruyo");
            Destroy(this.gameObject);

        }
       
    }

    public void SetOwner(LifeComponent owner) => _owner = owner;

    public void AddDamageOverTime(int TotalDamageToDeal, float TimeAmount)
        => _owner.AddDamageOverTime(TotalDamageToDeal, TimeAmount);

    public void AddHealOverTime(int totalHeal, float timeAmount) => _owner.AddHealOverTime(totalHeal, timeAmount);
 
    public int Heal(int HealAmount) => _owner.Heal(HealAmount);

    public DamageData TakeDamage(int dmgToDeal)
    {
        dmgToDeal = (int)(dmgToDeal * damageMultiplier);
        DamageData data = _owner.TakeDamage(dmgToDeal);
        data.wasCrit = isCritSpot;
        return data;
    }

    public DamageData TakeDamage(int dmgToDeal, Vector3 hitPoint)
    {
        _owner.SetHitPos(hitPoint);
        return TakeDamage(dmgToDeal);
    }

    public void AddKnockBack(Vector3 dir, float force)
    {
        _owner.AddKnockBack(dir,force);
    }
}
