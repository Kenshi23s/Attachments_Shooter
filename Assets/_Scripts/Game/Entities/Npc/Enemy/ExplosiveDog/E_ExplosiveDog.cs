using System.Linq;
using UnityEngine;
[RequireComponent(typeof(IA_Movement))]
[RequireComponent(typeof(DebugableObject))]
public class E_ExplosiveDog : Enemy
{
    DebugableObject _debug;
    StateMachine<string> _fsm;

    LifeComponent _health;
    public IA_Movement agent { get; private set; }

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

    private void Awake()
    {
        _debug= GetComponent<DebugableObject>();
        _fsm = new StateMachine<string>(); _fsm.Initialize(_debug);
        _health = GetComponent<LifeComponent>();
        agent = GetComponent<IA_Movement>();

        _health.OnKilled += () => Destroy(gameObject);

    }

    void Start()
    {
        agent.SetTargets(EnemyManager.instance.activeEnemies
            .Where(x => x.TryGetComponent(out IA_Movement y))
            .Select(x=> x.GetComponent<IA_Movement>()));

        _fsm.CreateState("Idle", new EDogState_Idle(agent, _fsm, _health));
        _fsm.CreateState("Pursuit", new EDogState_Pursuit(_fsm,agent, pursuitMaxSpeed, minJumpDistance));
        _fsm.CreateState("JumpAttack", new EDogState_JumpAttack(Explosion, agent._movement, unitsBehindPlayer, unitsAbovePlayer, _fsm));
        _fsm.ChangeState("Idle");
    }

    void Explosion()
    {
        _health.TakeDamage(_health.maxLife);
        _debug.Log("Explosion!");
    }
    void Initialize()
    {
        agent.SetTargets(EnemyManager.instance.activeEnemies
           .Where(x => x.TryGetComponent<IA_Movement>(out var T))
           .Select(x => x.GetComponent<IA_Movement>()));
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Execute();
    }
}
