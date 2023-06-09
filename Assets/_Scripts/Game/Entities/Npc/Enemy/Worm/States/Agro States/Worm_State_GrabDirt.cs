using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Worm_State_GrabDirt : Worm_State
{
    public Worm_State_GrabDirt(Enemy_Worm worm) : base(worm)
    {
    }


    float dmgRadius;
    int dmgCount,requiredDmg4explosion, damage;


    public override void OnEnter()
    {
        dmgCount = 0;
        _worm.health.OnTakeDamage += AddCount;

    }

    void AddCount(int value)
    {
        dmgCount += value;
        if (dmgCount>= requiredDmg4explosion)
        {
            dmgCount = 0;
            Explosion();
        }
    }

    void Explosion()
    {
        IEnumerable<IDamagable> col = _worm.transform.position.GetItemsOFTypeAround<IDamagable>(dmgRadius).Where(x => x.GetType() != typeof(Enemy));
        foreach (var item in col) item.TakeDamage(damage);
    }

    public override void OnExit()
    {
        _worm.health.OnTakeDamage -= AddCount;
    }
}
