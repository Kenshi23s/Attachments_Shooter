using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDogState_Pursuit : IState
{
    StateMachine<string> _fsm;
    IA_Movement _agent;
    float _pursuitSpeed;
    float _jumpRadius;


    public void GizmoShow()
    {
       
        Vector3 dir = _agent.transform.position - Player_Movement.position;
        Gizmos.color = Color.red;
        if (Physics.Raycast(_agent.transform.position, dir,out RaycastHit hit ,_jumpRadius, IA_Manager.instance.wall_Mask))
        {
            Gizmos.DrawLine(_agent.transform.position, hit.point);
        }
        else
        {
            Gizmos.DrawLine(_agent.transform.position, _agent.transform.position + dir.normalized * _jumpRadius);
        }
    }

    public void OnEnter()
    {
        _agent.SetDestination(Player_Movement.position);
        _agent.SetMaxSpeed(_pursuitSpeed);
    }

   
    public void OnUpdate()
    {
        if (Vector3.Distance(_agent.transform.position, Player_Movement.position)<_jumpRadius)        
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
