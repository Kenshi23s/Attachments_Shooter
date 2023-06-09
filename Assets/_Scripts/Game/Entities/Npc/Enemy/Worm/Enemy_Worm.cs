using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   
using Worm_AttackState = Worm_State_Attack.Worm_AttackState;


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

    #region Sight Settings
    [Header("Sight Settings")]
    [SerializeField]
    float _sightRadius, _loseSightRadius;
    #endregion

    #region Attack Settings
    [Header("Attack Settings")]

    // El rango de estos ataques siempre deberia ser menor a los de sight radius
    [SerializeField]
    float _meleeAttackRadius, _shootAcidRadius, _shootDirtRadius;
    [SerializeField, Min(0)] float _acidAttackFrequency, _dirtAttackFrequency;

    #region Melee
    [SerializeField] int _meleeDamage;
    [SerializeField] float _meleeKnockback;
    #endregion

    #region Acid 
    [SerializeField, Min(0)] int _acidDamage;
    #endregion

    #region Dirt
    [SerializeField] int _dirtDamage;
    #endregion

    #endregion

    #region Stun Settings
    [Header("Stun Settings")]

    [SerializeField]
    int _dmgNeeded4stun;
    int _stunDmgCount;

    public bool CanBeStunned => _stunDmgCount >= _dmgNeeded4stun;

    public float StunTime;

    void AddStunCharge(int dmgTaken) => _stunDmgCount += dmgTaken;

    public void StunWorm()
    {
        _stunDmgCount = 0;
        fsm.ChangeState(EWormStates.Stunned);
    }
    #endregion

    #region Flank Settings
    [Header("Flank Settings")]

    [SerializeField] float _flankDistance;
    [SerializeField] float _flankSpeed;
    #endregion

    #region Movement Settings
    [Header("Movement Settings")]

    [SerializeField]
    float _pursuitSpeed, _searchSpeed;
    #endregion


    [SerializeField]
    Transform _shootPivot;

    public Transform target;

    public override void ArtificialAwake()
    {
        AI_move = GetComponent<AI_Movement>();
        fsm = new StateMachine<EWormStates>();
        fsm.Initialize(debug);

        anim = GetComponent<Animator>();

        health.OnKilled += DieChange;
        health.OnTakeDamage += AddStunCharge;

        fsm.CreateState(EWormStates.Idle, new Worm_State_Idle(this));
        fsm.CreateState(EWormStates.Search, new Worm_State_Search(this));
        fsm.CreateState(EWormStates.Attack, new Worm_State_Attack(this));
        fsm.CreateState(EWormStates.Stunned, new Worm_State_Stunned(this));
        fsm.CreateState(EWormStates.Die, new Worm_State_Die(this));

        fsm.ChangeState(EWormStates.Idle);
    }

    void DieChange() => fsm.ChangeState(EWormStates.Die);


    Worm_AttackState ChooseBetweenAttacks() 
    {
        if (Random.Range(0, _acidAttackFrequency) >= Random.Range(0, _dirtAttackFrequency))
        {
            return Worm_AttackState.ShootAcid;
        }

        return Worm_AttackState.GrabDirt;
    }

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

