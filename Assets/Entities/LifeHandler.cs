using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LifeHandler 
{
    Entity _myEntity;

    [SerializeField]int _life;
    [SerializeField]int _maxLife;

    public int life=> _life;
    public int maxLife=> _maxLife;



    public void Initialize(Entity _entity)
    {
        _myEntity=_entity;
        _life = _maxLife;
    }
    public void Heal(int AddValue) => _life = Mathf.Clamp(_life, _life+ Mathf.Abs(AddValue), _maxLife);
    
    public int Damage(int value)
    {
        _life -= value;
        return _life;
    }

    //metodo heal OverTime

    //metodo Increase MaxHealht

    //metodo DecreaseMaxHealht

    //metodo DamageOverTime(?) si se hace, no deberia matarte, solo dejarte a 1 hp



    //void HealOverTime(int value ,float time)
    //{
    //    myEntity.everyTick += () => HealEveryTick





    //}

    //void HealEveryTick()
    //{
    //    time -= Time.deltaTime;
    //    if (time > 0)
    //        _life = (int)Mathf.Clamp(_life, (value * Time.deltaTime) + time, _maxLife);
    //    else
    //        myEntity.everyTick -= this.
    //}
}
