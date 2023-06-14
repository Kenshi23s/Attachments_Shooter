using FacundoColomboMethods;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(AI_Movement))]
[RequireComponent(typeof(DebugableObject))]
public class E_ExplosiveDog : Enemy
{
    StateMachine<EDogStates> _fsm;

    public enum EDogStates 
    {
        IDLE, PURSUIT, JUMP_ATTACK
    }

    public AI_Movement agent { get; private set; }

    #region Idle
    float alarmRadius;
    #endregion
    [Header("Pursuit")]

    [SerializeField]float pursuitMaxSpeed;
    [SerializeField]float minJumpDistance;

    [Header("JumpAttack")]
    [SerializeField,Tooltip("unidades arriba del player en el salto")] 
    float unitsAbovePlayer;


    [Header("Explosion")]
    [SerializeField]int _explosionDamage;
    [SerializeField]float _explosionRadius,triggerRadius;

    public override void ArtificialAwake()
    {
        _debug = GetComponent<DebugableObject>();
        _fsm = new StateMachine<EDogStates>(); _fsm.Initialize(_debug);
        health = GetComponent<LifeComponent>(); health.OnKilled += Explosion;

        agent = GetComponent<AI_Movement>();
    }

    void Start()
    {
        //agent.SetTargets(EnemyManager.instance.activeEnemies
        //    .Where(x => x.TryGetComponent(out AI_Movement y))
        //    .Select(x=> x.GetComponent<AI_Movement>()));

        _fsm.CreateState(EDogStates.IDLE, new EDogState_Idle(agent, _fsm, health));
        _fsm.CreateState(EDogStates.PURSUIT, new EDogState_Pursuit(_fsm,agent, pursuitMaxSpeed, minJumpDistance));
        _fsm.CreateState(EDogStates.JUMP_ATTACK, new EDogState_JumpAttack(triggerRadius, Explosion, agent.Movement, unitsAbovePlayer, _fsm));
        _fsm.ChangeState(EDogStates.IDLE);
    }

    void Explosion()
    {
        health.OnKilled -= Explosion;
        if (health.life > 0) { health.TakeDamage(int.MaxValue); return; }
       
        foreach (var item in transform.position.GetItemsOFTypeAround<IDamagable>(_explosionRadius))
        {
            item.TakeDamage(_explosionDamage);
        }
        _debug.Log("Explosion!");
        Destroy(gameObject);
        
    }
    void Initialize()
    {
        agent.SetTargets(EnemyManager.instance.activeEnemies
           .Where(x => x.TryGetComponent<AI_Movement>(out var T))
           .Select(x => x.GetComponent<AI_Movement>()));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _fsm.Execute();
    }

  
}
