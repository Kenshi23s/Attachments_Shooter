using System;
using UnityEngine;



[RequireComponent(typeof(FOVAgent))]
[RequireComponent(typeof(LifeComponent))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(AI_Movement))]
public class EggEscapeModel : MonoBehaviour
{
    public enum EggStates
    {
        Patrol,
        Escape,
        Stunned,
        Kidnapped
    }
    [System.Serializable]
    public struct EggStats
    {
        //ahora mismo "cualquiera" puede acceder al gamemode, estaria bueno despues volver
        //y dejar todo con getter y setter
        [Header("Components")]
        [NonSerialized]public EggGameChaseMode gameMode;
        public DebugableObject debug;

        // preguntarle a algun profe como podria dejar una variable por codigo
        // como get pero modificarla por inspector
        // (sin nesecidad de una variable como auxiliar)
        [Header("Stats")]
        public int requiredDmg4stun;
        public float stunTime;

        public float kidnapedTime;
        public float kidnapFollowRadius;

        [Header("Speeds")]
        [Range(0.1f,25)] public float kidnapSpeed, patrolSpeed, escapeSpeed;
      

    }

    LifeComponent myLife;
    FOVAgent _fov;
    AI_Movement _agent;

    StateMachine<EggStates> _fsm;
    public EggStates actualState => _fsm.actualStateKey;

    EggStats _eggStats;

    [Header("EggEscape")]
    [SerializeField] LineRenderer _playerLinking;

    Action onUpdate;



    //Action OnGrab;
    //Action OnRelease;


    //tendria que tener 4 estados:
    // Patrullar, hago la mia y sigo los waypoints, trato de no hacer el mismo camino q otro huevo en caso q haya(Obsoleto)
    // Escapar: vi al player con FOV al patrullar asi q me re tomo el palo en la direccion contraria a la q viene
    // Stuneado: el player me disparo y quede stuneado, no me puedo mover
    // Secuestrado: el player me agarro, desp de x segundos me libero y paso directo a el estado escapar

    public void Initialize(EggStats stats, Vector3 SpawnPos)
    {
        transform.position = SpawnPos;

        _eggStats = stats;
        SetLife();

        #region GetComponents
        DebugableObject _debug= GetComponent<DebugableObject>();
        _agent = GetComponent<AI_Movement>();
        _fov   = GetComponent<FOVAgent>();
        _fsm   = new StateMachine<EggStates>();


        #endregion

        #region DataSet
        EggState<EggStates>.EggStateData data;
        data._eggStats = _eggStats;
        data._fov = _fov;
        data._fsm = _fsm;
        data._agent = _agent;
        data.manual_Movement = _agent.Movement;
        data._eggStats.debug= _debug;
        #endregion

        #region Kidnapped CallBacks
        Action OnGrab = () => 
        {
            _playerLinking.gameObject.SetActive(true);
            onUpdate += UpdateLinkRope;
        };

        Action OnRelease = () => { _playerLinking.gameObject.SetActive(false); };
        #endregion

        #region Setting Finite State Machine
        _fsm.Initialize(_debug);
        _fsm.CreateState(EggStates.Patrol,    new EggState_Patrol(data));
        _fsm.CreateState(EggStates.Escape,    new EggState_Escape(data));
        _fsm.CreateState(EggStates.Stunned,   new EggState_Stunned(data));
        _fsm.CreateState(EggStates.Kidnapped, new EggState_Kidnaped(data, OnGrab,OnRelease));
        //_fsm.CreateState(EggStates.Kidnapped, /*new EggState_Escape(_eggStats, fov, agent, _fsm))*/null);
        #endregion

        _fsm.ChangeState(EggStates.Patrol);

    }
    public void SetLife()
    {
        myLife = GetComponent<LifeComponent>();
        myLife.SetNewMaxLife(_eggStats.requiredDmg4stun);
        myLife.OnKilled += Stun;

        myLife.Initialize();
    }

    void Update() => _fsm.Execute();

    void LateUpdate() =>  onUpdate?.Invoke();


    public void Grab()
    {
        if (_fsm.actualStateKey != EggStates.Kidnapped)                
            _fsm.ChangeState(EggStates.Kidnapped);                           
    }

    void UpdateLinkRope()
    {     
        _playerLinking.SetPosition(0, transform.position);
        _playerLinking.SetPosition(1, _eggStats.gameMode.playerPos);      
    }
    
    public void Stun()
    {
        if (_fsm.actualStateKey != EggStates.Kidnapped && _fsm.actualStateKey != EggStates.Stunned)
        {
            _fsm.ChangeState(EggStates.Stunned);
             myLife.canTakeDamage = false;
        }       
    }

    private void DrawEggGizmos()
    {
        if (_agent != null && _fsm != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_agent.Destination, transform.position);
            Gizmos.DrawWireSphere(_agent.Destination, 5f);
            _fsm.StateGizmos();
        }     
    }

}

