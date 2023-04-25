using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EggState;


[RequireComponent(typeof(NavMeshAgent))]
public class EggEscapeModel : MonoBehaviour,IDamagable,IPausable
{
    [System.Serializable]
    public struct EggStats
    {
        //ahora mismo "cualquiera" puede acceder al gamemode, estaria bueno despues volver
        //y dejar todo con getter y setter
        [Header("Components")]
        [NonSerialized]public EggGameChaseMode gameMode;

        // preguntarle a algun profe como podria dejar una variable por codigo
        // como get pero modificarla por inspector
        // (sin nesecidad de una variable como auxiliar)
        [Header("Stats")]
        public int requiredDmg4stun;
        public float stunTime;

        public float kidnapedTime;
        public float kidnapFollowRadius;
        public float patrolSpeed;
        public float escapeSpeed;

    }

    StateMachine<States> _fsm;
    public States actualState => _fsm.actualState;

    NavMeshAgent _agent;

   [SerializeField] FOVAgent _fov;
    EggStats _eggStats;

    [Header("EggEscape")]
    [SerializeField] int dmgCount = 0;
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


        #region GetComponents
        _agent = GetComponent<NavMeshAgent>();
        _fov=    GetComponent<FOVAgent>();
        _fsm =   new StateMachine<States>();
        #endregion

        #region DataSet
        EggStateData data;
        data._eggStats = _eggStats;
        data._fov = _fov;
        data._fsm = _fsm;
        data._agent = _agent;
        data._transform = transform;
        #endregion

        #region Kidnapped CallBacks
        Action OnGrab = () => 
        {
            _playerLinking.gameObject.SetActive(true);
            onUpdate += UpdateLink;
        };

        Action OnRelease = () => { _playerLinking.gameObject.SetActive(false); };
        #endregion

        #region Setting Finite State Machine
        _fsm.CreateState(States.Patrol,    new EggState_Patrol(data));
        _fsm.CreateState(States.Escape,    new EggState_Escape(data));
        _fsm.CreateState(States.Stunned,   new EggState_Stunned(data));
        _fsm.CreateState(States.Kidnapped, new EggState_Kidnaped(data, OnGrab,OnRelease));
        //_fsm.CreateState(EggStates.Kidnapped, /*new EggState_Escape(_eggStats, fov, agent, _fsm))*/null);
        #endregion

        _fsm.ChangeState(States.Patrol);

    }

    void Update() => _fsm.Execute();

    void LateUpdate()
    {
        onUpdate?.Invoke();
    }

    public void Grab()
    {
        if (_fsm.actualState != States.Kidnapped)
        {         
            _fsm.ChangeState(States.Kidnapped);
                 
        }      
    }

    void UpdateLink()
    {
        
        _playerLinking.SetPosition(0, transform.position);
        _playerLinking.SetPosition(1, _eggStats.gameMode.playerPos);
       
    }
    

    #region Polimorfism
    #region Damagable
    public int TakeDamage(int dmgValue)
    {
        Debug.Log(dmgValue);
        if (_fsm.actualState == States.Kidnapped || _fsm.actualState == States.Stunned) return 0;


        dmgCount += dmgValue;

        if (dmgCount > _eggStats.requiredDmg4stun)
        {
            Debug.Log(dmgCount);
            _fsm.ChangeState(States.Stunned);
            dmgCount= 0;
        }
        return dmgValue;       
    }
    public bool WasCrit() => _fsm.actualState == States.Stunned;

    public bool WasKilled() => false;
    #endregion

    #region Pausable
    public void Pause()
    {
        throw new System.NotImplementedException();
    }

    public void Resume()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (_agent != null && _fsm != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_agent.destination, transform.position);
            Gizmos.DrawWireSphere(_agent.destination, 5f);
            _fsm.StateGizmos();
        }
     
    }
    #endregion
}

