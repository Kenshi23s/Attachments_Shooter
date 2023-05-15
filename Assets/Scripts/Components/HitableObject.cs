using System;
using UnityEngine;

public class HitableObject : MonoBehaviour,IDamagable,IHealable
{
    LifeComponent _owner;

    [SerializeField,Tooltip("Marcar TRUE en caso de que sea punto critico")] 
    bool isCritSpot;
    [SerializeField, Range(0.1f,2f)] 
    float damageMultiplier;


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
}
