using FacundoColomboMethods;
using System;
using UnityEngine;
using UnityEngine.AI;
using static EggEscapeModel;

public class EggState_Patrol:EggState
{
 
   
    public EggState_Patrol(EggStateData data) : base(data) 
    { _getRandomWaypoint = () =>
    _eggStats.gameMode.waypoints[UnityEngine.Random.Range(0, _eggStats.gameMode.waypoints.Length)]; }
    //como podria acortar este func?, consultar a alguien
    Transform actualWaypoint;

    Func<Transform> _getRandomWaypoint;

    public override void OnEnter()
    {      
        actualWaypoint = _getRandomWaypoint?.Invoke();
        _agent.speed = _eggStats.patrolSpeed;       
    } 

    public override void OnUpdate()
    {
        _agent.SetDestination(actualWaypoint.position);
        float distance = Vector3.Distance(actualWaypoint.position, _agent.transform.position);

        if (distance < _eggStats.gameMode.interactRadius)
            actualWaypoint = _getRandomWaypoint();
    }

    public override void MakeDecision() 
    {
        if (_fov.inFOV(_eggStats.gameMode.playerPos)) 
            _fsm.ChangeState(EggStates.Escape);
    }

    public override void OnExit() { }
 
    public override void GizmoState()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(actualWaypoint.position, _eggStats.gameMode.interactRadius);
    }
  
}



