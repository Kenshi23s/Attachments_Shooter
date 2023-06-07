using FacundoColomboMethods;
using System;
using UnityEngine;
using UnityEngine.AI;
using static EggEscapeModel;

public class EggState_Patrol : EggState
{
 
   
    public EggState_Patrol(EggStateData data) : base(data) 
    {
      _getRandomWaypoint = () =>
      _eggStats.gameMode.waypoints[UnityEngine.Random.Range(0, _eggStats.gameMode.waypoints.Length)]; 
    }
    //como podria acortar este func?, consultar a alguien
    Transform _actualWaypoint;

    Func<Transform> _getRandomWaypoint;

    public override void OnEnter()
    {      
        _actualWaypoint = _getRandomWaypoint?.Invoke();

        _agent.SetMaxSpeed(_eggStats.patrolSpeed);
        _agent.SetDestination(_actualWaypoint.position);
    } 

    public override void OnUpdate()
    {
        _eggStats.debug.Log(_actualWaypoint.position.ToString()) ;
        float distance = Vector3.Distance(_actualWaypoint.position, myPos);     
        if (_eggStats.gameMode.interactRadius > distance)
        {
            _actualWaypoint = _getRandomWaypoint?.Invoke();
            _agent.SetDestination(_actualWaypoint.position);
        }

        if (_fov.IN_FOV(_eggStats.gameMode.playerPos))
            _fsm.ChangeState(States.Escape);
    }


    public override void OnExit() { }
 
    public override void GizmoShow()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_actualWaypoint.position, _eggStats.gameMode.interactRadius);
    }
  
}



