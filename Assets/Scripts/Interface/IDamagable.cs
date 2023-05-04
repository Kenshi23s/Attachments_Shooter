
public interface IDamagable 
{
    DamageData TakeDamage(int dmgToDeal);
    void AddHealOverTime(int TotalDamageToDeal,float TimeAmount);
    //bool WasKilled();
    //bool WasCrit();
}

public struct DamageData
{
    public int damageDealt;
    public bool wasKilled;
    public bool wasCrit;

    public DamageData(int damageDealt = 0, bool wasKilled = false, bool wasCrit = false)
    {
        this.damageDealt = damageDealt;
        this.wasKilled = wasKilled;
        this.wasCrit = wasCrit;
    }
}

public interface IHealable
{
    int Heal(int HealAmount);
    void AddHealOverTime(int totalHeal, float timeAmount);



}


