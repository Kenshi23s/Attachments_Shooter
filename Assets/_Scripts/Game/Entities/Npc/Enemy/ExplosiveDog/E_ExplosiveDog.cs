using FacundoColomboMethods;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(AI_Movement))]
[RequireComponent(typeof(DebugableObject))]
public class E_ExplosiveDog : Enemy
{
    StateMachine<string> _fsm;

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
    //me lo imagino como que salta, le erra y aterriza como derrapando(?) tendria que debatirlo con jocha
    [SerializeField,Tooltip("unidades detras del player al atterrizar" +
        "(habria que chequearlo con un raycast para que no quiera aterrizar en una pared)")] 
    float unitsBehindPlayer;

    #region OnJump
    float JumpSpeed;
    #endregion
    [SerializeField]int _explosionDamage;
    [SerializeField]float _explosionRadius;

    public override void ArtificialAwake()
    {
        debug = GetComponent<DebugableObject>();
        _fsm = new StateMachine<string>(); _fsm.Initialize(debug);
        health = GetComponent<LifeComponent>(); health.OnKilled += Explosion;

        agent = GetComponent<AI_Movement>();
    }

    void Start()
    {
        //agent.SetTargets(EnemyManager.instance.activeEnemies
        //    .Where(x => x.TryGetComponent(out AI_Movement y))
        //    .Select(x=> x.GetComponent<AI_Movement>()));

        _fsm.CreateState("Idle", new EDogState_Idle(agent, _fsm, health));
        _fsm.CreateState("Pursuit", new EDogState_Pursuit(_fsm,agent, pursuitMaxSpeed, minJumpDistance));
        _fsm.CreateState("JumpAttack", new EDogState_JumpAttack(Explosion, agent.Movement, unitsAbovePlayer, _fsm));
        _fsm.ChangeState("Idle");
    }

    void Explosion()
    {
        health.OnKilled -= Explosion;
        if (health.life > 0) { health.TakeDamage(int.MaxValue); return; }
       
        foreach (var item in transform.position.GetItemsOFTypeAround<IDamagable>(_explosionRadius))
        {
            item.TakeDamage(_explosionDamage);
        }
        debug.Log("Explosion!");
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
