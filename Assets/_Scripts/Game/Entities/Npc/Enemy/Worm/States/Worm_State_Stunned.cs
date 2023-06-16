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
        _worm.AI_move.CancelMovement();

        // Reiniciar timer de tiempo
        _stunTimeCount = 0;
        // Llamar a la animacion de stun
        _worm.anim.SetTrigger("Stun");
    }

    public override void OnUpdate()
    {
        _stunTimeCount += Time.deltaTime;

        if (_stunTimeCount >= _stunTime) 
            _worm.fsm.ChangeState(EWormStates.Attack);
    }

    public override void OnExit()
    {
    }
}
