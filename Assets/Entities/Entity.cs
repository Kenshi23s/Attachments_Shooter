using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour,IDamagable,IPausable
{

    public LifeHandler lifeHandler;
    public event Action everyTick; 
   
    public abstract void Pause();

    public abstract void Resume();

    public abstract int TakeDamage(int dmgDealt);

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
}
