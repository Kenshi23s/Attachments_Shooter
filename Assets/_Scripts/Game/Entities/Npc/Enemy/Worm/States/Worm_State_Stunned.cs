using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_Worm;

public class Worm_State_Stunned : Worm_State<EWormStates>
{
    public Worm_State_Stunned(Enemy_Worm worm) : base(worm) 
    {
        _stunTime = _worm.StunTime;
    }

    float _stunTime;
    float _stunTimeCount;

    public override void OnEnter()
    {
        // Reiniciar timer de tiempo
        _stunTimeCount = 0;

        // Setear animator a stunned
        _worm.anim.SetBool("Stunned", true);
    }

    public override void OnUpdate()
    {
        _stunTimeCount += Time.deltaTime;
        if (_stunTimeCount >= _stunTime) _worm.fsm.ChangeState(EWormStates.Attack);
    }

    public override void OnExit()
    {
        _worm.anim.SetBool("Stunned", false);
    }
}
