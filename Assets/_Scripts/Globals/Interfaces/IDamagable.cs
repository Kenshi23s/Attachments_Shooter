using UnityEngine;
public interface IDamagable 
{
    DamageData TakeDamage(int dmgToDeal);
    DamageData TakeDamage(int dmgToDeal,Vector3 hitPoint);
    void AddDamageOverTime(int TotalDamageToDeal, float TimeAmount);
}

public struct DamageData
{
    public int damageDealt;
    public bool wasKilled;
    public bool wasCrit;
    public IDamagable victim;

   
}

public interface IHealable
{
    int Heal(int HealAmount);
    void AddHealOverTime(int totalHeal, float timeAmount);



}


