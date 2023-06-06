using UnityEngine;
using static EggEscapeModel;
using UnityEngine.AI;
using FacundoColomboMethods;
using System.Linq;

public class EggState_Escape : EggState
{
    public EggState_Escape(EggStateData data) : base(data) { }

    Transform _actualWaypoint;

    Transform GetFurthestWaypoint() => _eggStats.gameMode.waypoints
        .OrderByDescending(x => Vector3.Distance(_agent.transform.position, x.transform.position))
        .First();

    public override void OnEnter()
    {
        _agent.SetMaxSpeed(_eggStats.escapeSpeed); 
        _actualWaypoint = GetFurthestWaypoint();
        _agent.SetDestination(_actualWaypoint.position);
    }

    public override void OnUpdate() 
    {
        float distance = Vector3.Distance(_actualWaypoint.position, _agent.transform.position);
        if (distance < _eggStats.gameMode.interactRadius)
        {
            if (!_fov.inFOV(_eggStats.gameMode.playerPos))
            {
                _fsm.ChangeState(States.Patrol);
                return;
            }
            OnEnter();
        }
    }

  
    public override void GizmoShow()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_actualWaypoint.position, _eggStats.gameMode.interactRadius);
    }

    public override void OnExit() {_actualWaypoint = null;}
    
}
