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
       Stunned, 
       Search
    }

    public StateMachine<EWormStates> fsm;
    public AI_Movement AI_move;
    public Animator anim;

    

    public Transform target;

    public override void ArtificialAwake()
    {
        AI_move = GetComponent<AI_Movement>();
        fsm = new StateMachine<EWormStates>();
        fsm.Initialize(debug);

        anim = GetComponent<Animator>();

        health.OnKilled += DieChange;
        health.OnTakeDamage += AddStunCharge;

        fsm.ChangeState(EWormStates.Idle);
    }

    void DieChange() => fsm.ChangeState(EWormStates.Die);

    #region Stun

    [SerializeField]
    int _dmgNeeded4stun;
    int _stunDmgCount;

    public bool CanBeStunned => _stunDmgCount >= _dmgNeeded4stun;

    void AddStunCharge(int dmgTaken) => _stunDmgCount += dmgTaken;
   
    public void StunWorm()
    {      
        _stunDmgCount = 0;
        fsm.ChangeState(EWormStates.Stunned);
    }
    #endregion

    [SerializeField]
    Transform _shootPivot;
    public void ShootAcid()
    {

    }

    public void AssignTarget(Transform newtarget) => target = newtarget != this ? newtarget : target;

    public void Destroy()
    {
        debug.Log("Mori");
        Destroy(gameObject);
    }
    

}

