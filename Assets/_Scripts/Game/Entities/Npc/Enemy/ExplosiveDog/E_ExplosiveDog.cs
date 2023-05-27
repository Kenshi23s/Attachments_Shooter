using System.Linq;
using UnityEngine;
[RequireComponent(typeof(IA_Movement))]
[RequireComponent(typeof(DebugableObject))]
public class E_ExplosiveDog : Enemy
{
    DebugableObject _debug;
    StateMachine<string> _fsm;
    public IA_Movement agent { get; private set; }

    #region Idle
    float alarmRadius;
    #endregion
    [Header("Pursuit")]

    [SerializeField]float pursuitMaxSpeed;
    [SerializeField]float minJumpDistance;
   

    #region OnJump
    float JumpSpeed;
    #endregion

    private void Awake()
    {
        _debug= GetComponent<DebugableObject>();
        _fsm = new StateMachine<string>(); _fsm.Initialize(_debug);

        agent = GetComponent<IA_Movement>();

    }

    void Start()
    {
        agent.SetTargets(EnemyManager.instance.activeEnemies
            .Where(x => x.TryGetComponent(out IA_Movement y))
            .Select(x=> x.GetComponent<IA_Movement>()));

        _fsm.CreateState("Idle", new EDogState_Idle(agent, _fsm));
        _fsm.CreateState("Pursuit", new EDogState_Pursuit(_fsm,agent, pursuitMaxSpeed, minJumpDistance));

        _fsm.ChangeState("Idle");
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
