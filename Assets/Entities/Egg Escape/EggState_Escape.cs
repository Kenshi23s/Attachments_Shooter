using UnityEngine;
using static EggEscapeModel;
using UnityEngine.AI;
using FacundoColomboMethods;

public class EggState_Escape : IState
{
  
     EggStats _stats;  
     FOVAgent _fov;
     NavMeshAgent _agent;
     StateMachine<EggStates> _fsm;

    Transform actualWaypoint;

    public EggState_Escape(EggStats _stats, FOVAgent _fov, NavMeshAgent _agent, StateMachine<EggStates> _fsm)
    {
        this._fsm = _fsm;
        this._stats = _stats;
        this._fov = _fov;
        this._agent = _agent;
       
    }

    Transform GetFurthestWaypoint() => _stats.gameMode.waypoints.GetFurthest(_agent.transform.position);

    public void OnEnter() => actualWaypoint = GetFurthestWaypoint();

    public void OnUpdate() => _agent.SetDestination(actualWaypoint.position);

    public void MakeDecision()
    {
        float distance = Vector3.Distance(actualWaypoint.position, _agent.transform.position);
        if (distance < _stats.gameMode.interactRadius)
        {
            if (_fov.inFOV(_stats.gameMode.playerPos))
            {
                _fsm.ChangeState(EggStates.Patrol);
                return;
            }
            actualWaypoint = GetFurthestWaypoint();
        }
    }
    public void GizmoState()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(actualWaypoint.position, _stats.gameMode.interactRadius);
    }

    public void OnExit() {actualWaypoint = null;}
    
}
