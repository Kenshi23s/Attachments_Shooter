using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_Worm;

public class Worm_State_Idle : Worm_State<EWormStates>
{
    public Worm_State_Idle(Enemy_Worm worm) : base(worm) {}

    public override void OnEnter()
    {
        _worm.anim.SetBool("Moving", false);
        _worm.health.OnTakeDamage += Change;
    }

    public override void OnUpdate()
    {
        if (_worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.SightRadius))       
            Change();     
    }

    void Change(int x = 0) => _worm.fsm.ChangeState(EWormStates.Search);  

    public override void OnExit()
    {
        _worm.health.OnTakeDamage -= Change;
    }

}
