using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDogState_Pursuit : IState
{
    StateMachine<string> _fsm;
    AI_Movement _agent;
    float _pursuitSpeed;
    float _jumpRadius;

    public EDogState_Pursuit(StateMachine<string> fsm, AI_Movement agent, float pursuitSpeed, float jumpRadius)
    {
        _fsm = fsm;
        _agent = agent;
        _pursuitSpeed = pursuitSpeed;
        _jumpRadius = jumpRadius;
    }

    public void OnEnter()
    {
        _agent.SetDestination(Player_Movement.position);
        _agent.SetMaxSpeed(_pursuitSpeed);
    }

    public void GizmoShow()
    {
        Vector3 dir = Player_Movement.position - _agent.transform.position;
        Gizmos.color = Color.red;
        if (Physics.Raycast(_agent.transform.position, dir, out RaycastHit hit, _jumpRadius, IA_Manager.instance.wall_Mask))
        {
            Gizmos.DrawLine(_agent.transform.position, hit.point);
            Gizmos.DrawWireSphere(hit.point, 2f);
        }
        else
        {
            Gizmos.DrawLine(_agent.transform.position, _agent.transform.position + dir.normalized * _jumpRadius);
            Gizmos.DrawWireSphere(_agent.transform.position + dir.normalized * _jumpRadius, 2f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_agent.transform.position, _jumpRadius);
    }

    public void OnUpdate()
    {
        _agent.SetDestination(Player_Movement.position);
        if (Vector3.Distance(_agent.transform.position, Player_Movement.position) <= _jumpRadius)      
        if (_agent.transform.position.InLineOffSight(Player_Movement.position,IA_Manager.instance.wall_Mask))
        {
           _fsm.ChangeState("JumpAttack");
        }      
    }

    public void OnExit()
    {
        _agent.ClearPath();
    }

}
