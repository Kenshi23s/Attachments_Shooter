using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Worm_AttackState = Worm_State_Attack.Worm_AttackState;


[RequireComponent(typeof(AI_Movement))]
//[RequireComponent(typeof(Animator))]
[SelectionBase]
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


    [NonSerialized] public AI_Movement AI_move;
    [NonSerialized] public Animator anim;
    public StateMachine<EWormStates> fsm;

    // Esto al final no seria necesario
    //[SerializedDictionary]
    //public SerializedDictionary<EWormStates, float> tailSpeedMultiplier = new SerializedDictionary<EWormStates, float>();

    #region Sight Settings
    [Header("Sight Settings")]
    [SerializeField]
    public float SightRadius;
    public float LoseSightRadius;
    #endregion

    #region Attack Settings
    [Header("Attack Settings")]

    // El radio de estos ataques siempre deberia ser menor a los de sight radius
    [SerializeField, Min(0)]
    public float MeleeAttackRadius;
    [SerializeField, Min(0)]
    public float ShootAcidRadius;
    [SerializeField, Min(0)]
    public float ShootDirtRadius;

    [SerializeField, Min(0)] float _acidAttackFrequency, _dirtAttackFrequency;

    #region Melee
    [Header("Melee")]
    [SerializeField] HitBox _meleeHitbox;
    [SerializeField, Min(0)] int _meleeDamage;
    [SerializeField, Min(0)] float _meleeKnockback;
    [SerializeField, Min(0)] float _meleeCooldown;
    [SerializeField, Min(0)] float _meleeStartTime = 0.4f;
    [SerializeField, Min(0)] float _meleeEndTime = 0.4f;
    #endregion

    #region Acid 
    [Header("Acid")]

    [SerializeField] Projectile_Acid _prefabAcid;
    Projectile_Acid _acidProjectile;

    [SerializeField, Min(0)] float _acidShootTime;
    [SerializeField, Min(0)] int _acidDamage;
    [SerializeField, Min(0)] float _acidCooldown;
    #endregion

    #region Dirt
    [Header("Dirt")]


    [SerializeField] Projectile_Rock _prefabDirt;
    Projectile_Rock _dirtProjectile;

    [SerializeField, Min(0)] int _dirtDamage = 20;
    [SerializeField, Min(0)] float _dirtKnockback = 30f;
    [SerializeField, Min(0)] float _dirtCooldown;
    [SerializeField, Min(0)] float _dirtRadius = 10f;
    [SerializeField, Min(0)] float _dirtGrabTime;
    [SerializeField, Min(0)] float _dirtShootTime;
    [SerializeField, Min(0)] float _dirtLaunchForce;
    #endregion

    #endregion

    public float DefenseKnockback = 10f;

    #region Stun Settings
    [Header("Stun Settings")]

    [SerializeField]
    int _dmgNeeded4stun;
    int _stunDmgCount;

    public bool CanBeStunned = true;

    public float StunTime;

    void AddStunCharge(int dmgTaken) {
        _stunDmgCount += dmgTaken;

        if (CanBeStunned && _stunDmgCount >= _dmgNeeded4stun)
            StunWorm();
    }

    public void StunWorm()
    {
        OnStun?.Invoke();
        _stunDmgCount = 0;
        fsm.ChangeState(EWormStates.Stunned);
    }
    #endregion

    #region Flank Settings
    [Header("Flank Settings")]

    public float FlankDistance;
    [SerializeField] float _flankSpeed;
    #endregion

    #region Movement Settings
    [Header("Movement Settings")]

    [SerializeField]
    float _pursuitSpeed, _searchSpeed;
    #endregion

    [Header("Pivots")]
    [SerializeField]
    Transform _shootPivot;

    [SerializeField]
    Transform _hitboxPos;
    
    public event Action OnStun;

    public Transform target;

    public Dictionary<string, float> AnimationLengths = new Dictionary<string, float>();

    public override void ArtificialAwake()
    {
        AI_move = GetComponent<AI_Movement>();
        anim = GetComponentInChildren<Animator>();

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            AnimationLengths.Add(clip.name, clip.length);
        }

        #region Initialize FSM

        fsm = new StateMachine<EWormStates>();
        fsm.Initialize(_debug); 

        health.OnKilled += DieChange;
        health.OnTakeDamage += AddStunCharge;

        // Creacion de estados
        fsm.CreateState(EWormStates.Idle, new Worm_State_Idle(this));
        fsm.CreateState(EWormStates.Search, new Worm_State_Search(this));
        fsm.CreateState(EWormStates.Attack, new Worm_State_Attack(this));
        fsm.CreateState(EWormStates.Stunned, new Worm_State_Stunned(this));
        fsm.CreateState(EWormStates.Die, new Worm_State_Die(this));

        // Estado Inicial
        fsm.ChangeState(EWormStates.Idle);
        #endregion

    } 
    private void Start()
    {
        #region CreateHitbox       
        _meleeHitbox.SetOwner(gameObject);
        _meleeHitbox.DeactivateHitBox();
        _meleeHitbox.EnemyHit += (x) =>
        {
            _debug.Log("le pegue con el melee");
            x.TakeDamage(_meleeDamage);
            Vector3 dir = x.Position() - transform.position;
            x.AddKnockBack(Vector3.down + dir.normalized * _meleeKnockback);

        };
        #endregion
    }

    private void Update()
    {
        fsm.Execute();
    }

    void DieChange() => fsm.ChangeState(EWormStates.Die);

    public Worm_AttackState ChooseBetweenAttacks() 
    {
        if (Random.Range(0, _acidAttackFrequency) >= Random.Range(0, _dirtAttackFrequency))
            return Worm_AttackState.ShootAcid;

        return Worm_AttackState.GrabDirt;
    }

    // Se llama por animacion
    public IEnumerator ShootAcid()
    {
        yield return new WaitForSeconds(_acidShootTime);
        var x = Instantiate(_prefabAcid, _shootPivot.position, Quaternion.identity);
        x.Initialize(Tuple.Create(gameObject, _acidDamage, target.position - _shootPivot.position));
    }
    
    public void CancelShootAcid()
    {
        StopCoroutine(ShootAcid());

        if (_acidProjectile != null)
        {
            // NOTA: Mas adelante esto regresaria a una pool
            Destroy(_acidProjectile);
            _acidProjectile = null;
        }
    }

    public IEnumerator GrabDirt() 
    {
        //grab
        yield return new WaitForSeconds(_dirtGrabTime);
        _dirtProjectile = Instantiate(_prefabDirt, _shootPivot.position, Quaternion.identity);
        _dirtProjectile.Iniitialize(gameObject, _dirtRadius);
        _dirtProjectile.transform.SetParent(_shootPivot);
    }

    public void CancelGrabDirt() 
    {
        StopCoroutine(GrabDirt());
        
        if (_dirtProjectile != null)
        {
            // NOTA: Mas adelante esto regresaria a una pool
            Destroy(_dirtProjectile);
            _dirtProjectile = null;
        }
    }

    public IEnumerator ShootDirt() 
    {
        //shoot
        yield return new WaitForSeconds(_dirtShootTime);

        Projectile_Rock aux = _dirtProjectile;
        aux.transform.parent = null;
        aux.onExplosion += (col) =>
        {
            foreach (var x in col)
            {
                if (x.TryGetComponent(out IDamagable y))
                {
                    y.TakeDamage(_dirtDamage);
                    Vector3 dir = x.transform.position - aux.transform.position;
                    y.AddKnockBack(dir.normalized * _dirtKnockback);
                }
            }
        };

        Vector3 dir = Player_Movement.position - _shootPivot.position;
        _dirtProjectile.LaunchProjectile(dir.normalized * _dirtLaunchForce);
        _dirtProjectile = null;
    }

    public void CancelShootDirt() 
    {
        StopCoroutine(ShootDirt());

        if (_dirtProjectile != null)
        {
            // NOTA: Mas adelante esto regresaria a una pool
            Destroy(_dirtProjectile);
            _dirtProjectile = null;
        }
    }

    public IEnumerator SpawnMeleeHitbox()
    {
        _debug.Log("Spawneo la hitBox");
        yield return new WaitForSeconds(_meleeStartTime);
        _meleeHitbox.ActivateHitbox();
        yield return new WaitForSeconds(_meleeEndTime);
        _meleeHitbox.DeactivateHitBox();
    }

    public void CancelMelee() 
    {
        StopCoroutine(SpawnMeleeHitbox());
        _meleeHitbox.DeactivateHitBox();
    }

    public void AssignTarget(Transform newtarget)
    {
        Debug.Log(newtarget);
        target = newtarget != this ? newtarget : target;
        Debug.Log("Asigno a "+target.ToString()+" como mi target");
    }

    public void Destroy()
    {
        _debug.Log("Mori");
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SightRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, LoseSightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MeleeAttackRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, ShootAcidRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, ShootDirtRadius);
    }
}

