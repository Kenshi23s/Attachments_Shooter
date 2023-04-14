using FacundoColomboMethods;
using UnityEngine;
using UnityEngine.AI;
using static EggEscapeModel;

public class EggState_Patrol:IState
{
    Transform actualWaypoint;

    // lo deberia tener la clase de gamemode (?)

    //se debe pasar por constructor

    FOVAgent fov;
    NavMeshAgent agent;
    StateMachine<EggStates> fsm;
    EggStats eggStats;

    public EggState_Patrol(EggStats eggStats, FOVAgent fov, NavMeshAgent agent, StateMachine<EggStates> fsm)
    {
        this.fov = fov;
        this.agent = agent;
        this.fsm = fsm;
        this.eggStats = eggStats;
    }

    public void OnEnter() => actualWaypoint = GetRandomWaypoint();

    public void OnUpdate()
    {
        agent.SetDestination(actualWaypoint.position);
        float distance = Vector3.Distance(actualWaypoint.position, agent.transform.position);

        if (distance < eggStats.gameMode.interactRadius)
            actualWaypoint = GetRandomWaypoint();
    }

    Transform GetRandomWaypoint() =>
   eggStats.gameMode.waypoints[Random.Range(0, eggStats.gameMode.waypoints.Length - 1)];

    public void MakeDecision() 
    {
        if (fov.inFOV(eggStats.gameMode.playerPos)) 
            fsm.ChangeState(EggStates.Escape);
    }

    public void OnExit() { }
 

    public void GizmoState()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(actualWaypoint.position, eggStats.gameMode.interactRadius);
    }
  
}



