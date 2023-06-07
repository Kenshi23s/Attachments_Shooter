using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_Worm;

public class Worm_State_Search : Worm_State
{
    public Worm_State_Search(Enemy_Worm worm) : base(worm)
    {
    }


    public override void OnEnter()
    {
        Vector3 actualPos = _worm.transform.position;
        _worm.AI_move.SetDestination(Player_Movement.position);
        _worm.anim.SetFloat("Speed", 1);

        _worm.AI_move.OnDestinationReach += () =>
        {
            _worm.AI_move.SetDestination(actualPos);
            _worm.AI_move.OnDestinationReach += () => _worm.fsm.ChangeState(EWormStates.Idle);
        };

    }

    public override void OnUpdate()
    {
        if (_worm.AI_move.FOV.IN_FOV(Player_Movement.position))
            _worm.fsm.ChangeState(EWormStates.Attack);
        
    }

    public override void OnExit() => _worm.AI_move.ClearPath();
  
}
