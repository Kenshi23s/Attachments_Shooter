using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI_Movement))]
public class Enemy_Worm : Enemy
{
    public enum EWormStates
    {
        Idle,
        Pursuit,
        Stunned,
        Die,
        Shoot,
        Melee,
        Underground,
        Emerge,
        Submerge
    }
    StateMachine<EWormStates> _fsm;
    AI_Movement AI_move;


    public override void ArtificialAwake()
    {
        AI_move = GetComponent<AI_Movement>();
        _fsm = new StateMachine<EWormStates>();
        _fsm.Initialize(debug);

        health.OnKilled += DieChange;
        health.OnTakeDamage += Stun;

        _fsm.ChangeState(EWormStates.Underground);
    }

    void DieChange() => _fsm.ChangeState(EWormStates.Die);
   

    int stunCount;
    int dmgNeeded4stun;

    void Stun(int dmgTaken)
    {
        stunCount += dmgTaken;
        if (stunCount>= dmgNeeded4stun && (_fsm.actualState != EWormStates.Melee))
        {
            stunCount = 0;
            _fsm.ChangeState(EWormStates.Stunned);
        }
    }

    [SerializeField]
    Transform _shootPivot;
    public void Shoot(Vector3 player)
    {

    }
    
}

