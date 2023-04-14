using System;

using UnityEngine;

public abstract class Entity : MonoBehaviour, IPausable, IDamagable
{

    public LifeHandler lifeHandler;
    public event Action everyTick;

    #region Events

    public event Action OnDeath;
    public event Action<int> onTakeDamage;
    #endregion

    public abstract void Pause();

    public abstract void Resume();
    // se debe devolver el valor con el que se restara la vida(si tiene armadura o toma menos daño por x motivo)
    public abstract int OnTakeDamage(int dmgDealt);

    public virtual int TakeDamage(int dmgDealt)
    {   
        int aux = lifeHandler.Damage(OnTakeDamage(dmgDealt));
        onTakeDamage?.Invoke(aux);
        return aux;

    }

    public abstract bool WasCrit();

    public abstract bool WasKilled();

    private void Awake()
    {
        lifeHandler.Initialize(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        everyTick?.Invoke();
    }

    //void
}
