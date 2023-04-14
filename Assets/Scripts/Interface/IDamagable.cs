
public interface IDamagable 
{
    public virtual int TakeDamage(int dmgDealt) { return default; }
    bool WasKilled();
    bool WasCrit();
}
