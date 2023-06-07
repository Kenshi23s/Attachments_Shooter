using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AI_Movement))]
[RequireComponent(typeof(Animator))]
public class Enemy_Worm : Enemy
{
    public enum EWormStates
    {
       Idle,
       Attack,
       Die,
       Stunned     
    }
    StateMachine<EWormStates> _fsm;
    AI_Movement AI_move;
   public Animator anim;

    public override void ArtificialAwake()
    {
        AI_move = GetComponent<AI_Movement>();
        _fsm = new StateMachine<EWormStates>();
        _fsm.Initialize(debug);

        anim = GetComponent<Animator>();

        health.OnKilled += DieChange;
        health.OnTakeDamage += AddStunCharge;

        _fsm.ChangeState(EWormStates.Idle);
    }

    void DieChange() => _fsm.ChangeState(EWormStates.Die);

    #region Stun

    [SerializeField]
    int _dmgNeeded4stun;
    int _stunDmgCount;

    public bool CanBeStunned => _stunDmgCount >= _dmgNeeded4stun;

    void AddStunCharge(int dmgTaken) => _stunDmgCount += dmgTaken;
   
    public void StunWorm()
    {      
        _stunDmgCount = 0;
        _fsm.ChangeState(EWormStates.Stunned);
    }
    #endregion

    [SerializeField]
    Transform _shootPivot;
    public void Shoot(Vector3 player)
    {

    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
    

}

