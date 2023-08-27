using FacundoColomboMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static E_ExplosiveDog;

public class EDogState_Pursuit : IState<EDogStates>
{
    StateMachine<EDogStates> _fsm;
    AI_Movement _agent;
    float _pursuitSpeed;
    float _jumpRadius;
    Action blinkOn;
    public EDogState_Pursuit(Action blinkOn,StateMachine<EDogStates> fsm, AI_Movement agent, float pursuitSpeed, float jumpRadius)
    {
        this.blinkOn = blinkOn;
        _fsm = fsm;
        _agent = agent;
        _pursuitSpeed = pursuitSpeed;
        _jumpRadius = jumpRadius;
    }

    public void OnEnter()
    {
        _agent.SetDestination(Player_Handler.position);
        _agent.SetMaxSpeed(_pursuitSpeed);
    }

   

    public void OnUpdate()
    {
        if (_agent.Movement.maxForce == 0)
        {
            Vector3 dir  = Player_Handler.position-_agent.transform.position;
            _agent.transform.forward = new Vector3(dir.x,0,dir.z);
        }
        else
        {
            _agent.SetDestination(Player_Handler.position);
            if (Vector3.Distance(_agent.transform.position, Player_Handler.position) <= _jumpRadius)
                if (_agent.transform.position.InLineOffSight(Player_Handler.position, AI_Manager.instance.WallMask))
                {
                    _fsm.ChangeState(EDogStates.JUMP_ATTACK);
                }
        }
       
    }

    public void OnExit()
    {
        _agent.CancelMovement();
    }


    public void SetStateMachine(StateMachine<EDogStates> fsm)
    {
    }

    public void GizmoShow()
    {
        Vector3 dir = Player_Handler.position - _agent.transform.position;
        Gizmos.color = Color.red;
        if (Physics.Raycast(_agent.transform.position, dir, out RaycastHit hit, _jumpRadius, AI_Manager.instance.WallMask))
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
}
