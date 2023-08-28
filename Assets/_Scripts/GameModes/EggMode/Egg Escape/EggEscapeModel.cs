using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FOVAgent))]
[RequireComponent(typeof(LifeComponent))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(NewAIMovement))]
[RequireComponent(typeof(GrabableObject))]
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
        [Header("StunnedStats")]
        public int requiredDmg4stun;
        public float stunTime;

        [Header("Kidnapped Stats")]
        public float kidnapedTime;
        public float kidnapFollowRadius, kidnapSpeed;

        [Header("Patrol")]
        [Range(0f,25)] public float patrolSpeed;
        [Header("Escape")]
        [Range(0.1f, 25)] public float escapeSpeed;


    }

    LifeComponent myLife;
    FOVAgent _fov;
    NewAIMovement _agent;

    public InteractableComponent InteractComponent { get; private set; }
    public GrabableObject GrabableComponent { get; private set; }
    public Egg_VFXHandler VFX { get; private set; }
    public GameObject view;

    StateMachine<EggStates> _fsm;
    public EggStates actualState => _fsm.actualStateKey;

    public EggStats CurrentEggStats { get; private set; }


    public Action OnUpdate;

    public UnityEvent OnGrab, OnRelease;


    //Action OnGrab;
    //Action OnRelease;
    private void Awake()
    {
       
        InteractComponent = GetComponent<InteractableComponent>();
        InteractComponent.SetFocusCondition(() => _fsm.actualStateKey != EggStates.Kidnapped);

        GrabableComponent = GetComponent<GrabableObject>();

        GrabableComponent.OnGrab.AddListener(DisableEggLogic);
        GrabableComponent.OnRelease.AddListener(EnableEggLogic);

        _agent = GetComponent<NewAIMovement>();
        _fov = GetComponent<FOVAgent>();
        _fsm = new StateMachine<EggStates>();
        enabled = false;
    }

    public void DisableEggLogic() 
    {
        _agent.ManualMovement.ClearForces();
        //_agent.ManualMovement.Rigidbody.isKinematic = true;
        _agent.ManualMovement.Rigidbody.useGravity = false;
        _agent.ManualMovement.enabled = false;
        _agent.enabled = false;
        enabled = false;

        GetComponent<BoxCollider>().enabled = false;

    }

    public void EnableEggLogic()
    {
        //_agent.ManualMovement.Rigidbody.isKinematic = false;
        _agent.ManualMovement.Rigidbody.useGravity = true;
        _agent.ManualMovement.enabled = true;
        _agent.enabled = true;
        enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    //tendria que tener 4 estados:
    // Patrullar, hago la mia y sigo los waypoints, trato de no hacer el mismo camino q otro huevo en caso q haya(Obsoleto)
    // Escapar: vi al player con FOV al patrullar asi q me re tomo el palo en la direccion contraria a la q viene
    // Stuneado: el player me disparo y quede stuneado, no me puedo mover
    // Secuestrado: el player me agarro, desp de x segundos me libero y paso directo a el estado escapar

    public void Initialize(EggStats stats, Vector3 SpawnPos)
    {
        transform.position = SpawnPos;

        CurrentEggStats = stats;
        SetLife();
        #region GetComponents
        DebugableObject _debug = GetComponent<DebugableObject>();
      

        _debug.Log("Inicializo " + gameObject.name);
        #endregion

        #region DataSet
        EggState<EggStates>.EggStateData data;
        data._eggStats = CurrentEggStats;
        data._fov = _fov;
        data._fsm = _fsm;
        data._agent = _agent;
        data.manual_Movement = _agent.ManualMovement;
        data._eggStats.debug= _debug;
        #endregion

   

        #region Setting Finite State Machine
        _fsm.Initialize(_debug);
        _fsm.CreateState(EggStates.Patrol,    new EggState_Patrol(data));
        _fsm.CreateState(EggStates.Escape,    new EggState_Escape(data));
        _fsm.CreateState(EggStates.Stunned,   new EggState_Stunned(data));
        _fsm.CreateState(EggStates.Kidnapped, new EggState_Kidnaped(data, OnGrab ,OnRelease));
        //_fsm.CreateState(EggStates.Kidnapped, /*new EggState_Escape(_eggStats, fov, agent, _fsm))*/null);
        #endregion

        _fsm.ChangeState(EggStates.Patrol);
        VFX = GetComponentInChildren<Egg_VFXHandler>(); VFX.Initialize(this);
        enabled = true;

    }

    public void SetLife()
    {
        myLife = GetComponent<LifeComponent>();
        myLife.SetNewMaxLife(CurrentEggStats.requiredDmg4stun);
        myLife.OnTakeDamage.AddListener(Escape);
        myLife.OnKilled.AddListener(Stun);
        myLife.Initialize();
    }

    public void Escape(int x)
    {
        if (_fsm.actualStateKey == EggStates.Patrol && _fsm.actualStateKey != EggStates.Escape)
        {
            _fsm.ChangeState(EggStates.Escape);
        }
    }

    void Update() => _fsm.Execute();

    void LateUpdate() => OnUpdate?.Invoke();

    public void Grab()
    {
        if (_fsm.actualStateKey != EggStates.Kidnapped)                
            _fsm.ChangeState(EggStates.Kidnapped);                           
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

    private void OnDestroy()
    {
        CurrentEggStats.gameMode.eggsEscaping.Remove(this);
    }

}

