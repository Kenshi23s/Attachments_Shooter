using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TickEventsManager;

public class LifeComponent : MonoBehaviour,IDamagable,IHealable
{
    [SerializeField] int _life;
    [SerializeField] int _maxLife;

    public int life => _life;
    public int maxLife => _maxLife;

    [SerializeField] public bool canTakeDamage = true;
    [SerializeField] public bool canBeHealed = true;

    public event Action OnHeal;
    public event Action<int> OnTakeDamage;
    public event Action OnKilled;


    public void SetNewMaxLife(int value)
    {
        _maxLife=value;
    }

    public void Initialize()
    {
        _life = _maxLife;
        enabled = false;
    }
   
    #region DamageSide
    public DamageData TakeDamage(int dmgDealt)
    {
        DamageData data = new DamageData();
        if (!canTakeDamage) return data;
        Debug.Log($"{gameObject.name} recibio {dmgDealt} de daño ");


        _life -= Mathf.Abs(dmgDealt); OnTakeDamage?.Invoke(dmgDealt);

        if (_life <= 0)
        {
            OnKilled?.Invoke();
            data.wasKilled = true;
        }
        data.damageDealt = dmgDealt;
        return data;
    }

    public void AddDamageOverTime(int TotalDamageToDeal, float TimeAmount)
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
    public int Heal(int HealAmount)
    {

        if (!canBeHealed) return 0;

        _life += Mathf.Abs(HealAmount);

        if (_life > _maxLife) _life = _maxLife;

        OnHeal?.Invoke();
        return HealAmount;
    }

    public void AddHealOverTime(int totalHeal, float timeAmount)
    {
        //No se puede morir de daño OverTime, siempre te deja a un hit
        //, se podria ver q para los enemigos si mueran de esto
       
        int HealPerTick = (int)(totalHeal / timeAmount);

        Action action= () => 
        {
            int dmgToDeal = life - HealPerTick > 0 ? HealPerTick : 0;
            TakeDamage(dmgToDeal);
        };

        AddLifeEvent(action, timeAmount);
    }
    #endregion

    void AddLifeEvent(Action action,float timeAmount)
    {

        TickEvents newDamageEvent = new TickEvents();
        newDamageEvent.isTimeBased = true;
        newDamageEvent.timeStart = timeAmount;
        TickEventsManager.instance.AddAction(newDamageEvent);
    }

   

   
}

