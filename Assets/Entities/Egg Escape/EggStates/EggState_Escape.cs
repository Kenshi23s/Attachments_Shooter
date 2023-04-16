using UnityEngine;
using static EggEscapeModel;
using UnityEngine.AI;
using FacundoColomboMethods;

public class EggState_Escape : EggState
{
    public EggState_Escape(EggStateData data) : base(data) { }

    Transform actualWaypoint;

   

    Transform GetFurthestWaypoint() => _eggStats.gameMode.waypoints.GetFurthest(_agent.transform.position);

    public override void OnEnter()
    {
         actualWaypoint = GetFurthestWaypoint();
        _agent.speed = _eggStats.escapeSpeed;
    }

    public override void OnUpdate() => _agent.SetDestination(actualWaypoint.position);

    public override void MakeDecision()
    {
        float distance = Vector3.Distance(actualWaypoint.position, _agent.transform.position);
        if (distance < _eggStats.gameMode.interactRadius)
        {
            if (_fov.inFOV(_eggStats.gameMode.playerPos))
            {
                _fsm.ChangeState(EggStates.Patrol);
                return;
            }
            actualWaypoint = GetFurthestWaypoint();
        }
    }
    public override void GizmoState()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(actualWaypoint.position, _eggStats.gameMode.interactRadius);
    }

    public override void OnExit() {actualWaypoint = null;}
    
}
