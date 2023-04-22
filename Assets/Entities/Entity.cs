using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IPausable, IDamagable
{
   
    [SerializeField]protected LifeHandler lifeHandler;
    public event Action everyTick;

    #region Events
    //public event Action<int> onTakeDamage;
    #endregion

    public abstract void Pause();

    public abstract void Resume();
    /// <summary>
    /// se debe devolver el valor con el que se restara la vida(si tiene armadura o toma menos daño por x motivo)
    /// </summary>
    /// <param name="dmgDealt"></param>
    /// <returns></returns>
   


    public abstract void OnDeath();

    public abstract bool WasCrit();

    public abstract bool WasKilled();

    protected abstract int OnTakeDamage(int dmgDealt);

    public virtual int TakeDamage(int dmgDealt)
    {   
        int aux = lifeHandler.Damage(OnTakeDamage(dmgDealt));
        //onTakeDamage?.Invoke(aux);
        return aux;

    }

    protected virtual void Awake() => lifeHandler.Initialize(this, OnDeath);

    void Update() => everyTick?.Invoke();
 
}
