using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_GrabDirt : Worm_State<Worm_AttackState>
{
    public Worm_State_GrabDirt(Enemy_Worm worm) : base(worm)
    {
    }


    float dmgRadius;
    int dmgCount,requiredDmg4explosion, damage;
    int auxDmgResist;

    public override void OnEnter()
    {
        dmgCount = 0;
        _worm.health.OnTakeDamage += AddCount;
        auxDmgResist = _worm.health.dmgResist;
        _worm.health.dmgResist = 2;

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
        foreach (var item in col) 
        { 
            item.TakeDamage(damage);
            Vector3 dir = item.Position() - _worm.transform.position;
            item.AddKnockBack(dir * 10f);
        }
    }

    public override void OnExit()
    {
        _worm.health.OnTakeDamage -= AddCount;
        _worm.health.dmgResist = auxDmgResist;
    }

    public override void GizmoShow()
    {
        Gizmos.DrawWireSphere(_worm.transform.position, dmgRadius);
    }
}
