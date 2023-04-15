using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EggEscapeModel : InteractableObject,IDamagable,IPausable
{
    [System.Serializable]
    public struct EggStats
    {
        [Header("Components")]
        public EggGameChaseMode gameMode;

        [Header("Stats")]
        public int requiredDmg4stun;
        public int stunTime;
        public float speed;
        public float escapeSpeed;

    }

    
    NavMeshAgent agent;
    StateMachine<EggStates> _fsm;
    FOVAgent fov;

    [Header("EggEscape")]
    [SerializeField] int dmgCount=0;

    [SerializeField] bool stunned=false;
    
    EggStats _eggStats;

    public enum EggStates
    {
        Patrol,
        Escape,
        Kidnapped
    }
    //tendria que tener 3 estados:
    // Patrullar, hago la mia y sigo los waypoints, trato de no hacer el mismo camino q otro huevo en caso q haya(Obsoleto)
    // Escapar: vi al player con FOV al patrullar asi q me re tomo el palo en la direccion contraria a la q viene
    // Liberarse: el player me agarro, desp de x segundos me libero y paso directo a el estado escapar
   

    public void Initialize(EggStats stats, Vector3 SpawnPos)
    {
        transform.position = SpawnPos;
        stunned = false;

        _eggStats = stats;

        
        #region Agent Set
        agent = GetComponent<NavMeshAgent>();
        agent.speed=_eggStats.speed;
        #endregion

         fov = new FOVAgent(transform);
        _fsm = new StateMachine<EggStates>();
       

        #region Setting Finite State Machine
        _fsm.CreateState(EggStates.Patrol, new EggState_Patrol   (_eggStats, fov, agent, _fsm));
        _fsm.CreateState(EggStates.Escape, new EggState_Escape   (_eggStats, fov, agent, _fsm));
        //_fsm.CreateState(EggStates.Kidnapped, /*new EggState_Escape(_eggStats, fov, agent, _fsm))*/null);
        #endregion

        _fsm.ChangeState(EggStates.Patrol);

    }

    void Update() => _fsm.Execute();

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

    public int TakeDamage(int dmgValue)
    {
        if (stunned)
            return 0;

        dmgCount += dmgValue;
        if (dmgCount > _eggStats.requiredDmg4stun)
         StartCoroutine(StunnedCooldown());

        return dmgValue;       
    }

    IEnumerator StunnedCooldown()
    {
        agent.speed = 0;
        stunned = true;

        yield return new WaitForSeconds(_eggStats.stunTime);
        

        stunned= false;
        agent.speed = _eggStats.speed;
    }

    public  bool WasCrit() => stunned;
   
    public  bool WasKilled()=> false;
}
