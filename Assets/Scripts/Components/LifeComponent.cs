using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TickEventsManager;
[RequireComponent(typeof(DebugableObject))]
public class LifeComponent : MonoBehaviour,IDamagable,IHealable
{
    DebugableObject _debug;

    [SerializeField] int _life = 100;
    [SerializeField] int _maxLife = 100;

    public int life => _life;
    public int maxLife => _maxLife;

    [SerializeField,Range(0.1f,2)] 
    float _dmgMultiplier=1f;


    [SerializeField] public bool canTakeDamage = true;
    [SerializeField] public bool canBeHealed = true;

    public event Action OnHeal;
    public event Action<int> OnTakeDamage;
    public event Action OnKilled;

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        // por si tenes hijos que pueden hacer de 
        foreach (var item in GetComponentsInChildren<HitableObject>()) 
            item.SetOwner(this);
    }

    public void SetNewMaxLife(int value) => _maxLife = Mathf.Clamp(value,1,int.MaxValue);
    

    public void Initialize()
    {
        _life = _maxLife;
        enabled = false;
    }
   
    #region DamageSide
    public virtual DamageData TakeDamage(int dmgDealt)
    {
        DamageData data = new DamageData();
        if (!canTakeDamage) return data;
       


        _life -= (int)(Mathf.Abs(dmgDealt) * _dmgMultiplier); OnTakeDamage?.Invoke(dmgDealt);
        _debug.Log($" recibio {dmgDealt * _dmgMultiplier} de daño ");

        if (_life <= 0)
        {
            OnKilled?.Invoke();
            data.wasKilled = true;
        }
        data.damageDealt = dmgDealt;
        return data;
    }

    public virtual void AddDamageOverTime(int TotalDamageToDeal, float TimeAmount)
    {   
        int damagePerTick = (int)(TotalDamageToDeal / TimeAmount);

        Action action = () => 
        {
            int dmgToDeal =  life - damagePerTick > 0 ? damagePerTick : 0;
            TakeDamage(dmgToDeal);
        };

        AddLifeEvent(action, TimeAmount);
    }
    #endregion

    #region HealingSide
    public virtual int Heal(int HealAmount)
    {

        if (!canBeHealed) return 0;

        _life += Mathf.Abs(HealAmount);

        OnHeal?.Invoke();
        if (_life > _maxLife) _life = _maxLife;

        return HealAmount;
    }

    public void AddHealOverTime(int totalHeal, float timeAmount)
    {
        //No se puede morir de daño OverTime, siempre te deja a un hit
        //, se podria ver q para los enemigos si mueran de esto
       
        int HealPerTick = (int)(totalHeal / timeAmount);

        Action action = () => 
        {
            int dmgToDeal = life - HealPerTick > 0 ? HealPerTick : 0;
            TakeDamage(dmgToDeal);
        };

        AddLifeEvent(action, timeAmount);
    }
    #endregion

    void AddLifeEvent(Action action,float timeAmount)
    {

        TickEvent newDamageEvent = new TickEvent(); 

        newDamageEvent.OnTickAction= action;
        newDamageEvent.isTimeBased = true;
        newDamageEvent.timeStart = timeAmount;

        TickEventsManager.instance.AddAction(newDamageEvent);
    }

   

   
}



