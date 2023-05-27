using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDogState_JumpAttack : IState
{

    Action _explosion;   
    Physics_Movement _move;
    float unitsBehindPlayer;

    float unitsAbovePlayer=2.5f;
    StateMachine<string> _fsm;

    int count = 0;
    Vector3[] pos = new Vector3[2];

    public EDogState_JumpAttack(Action _explosion, Physics_Movement _move, float unitsBehindPlayer, float unitsAbovePlayer, StateMachine<string> _fsm)
    {
        this._explosion = _explosion;
        this._move = _move;
        this.unitsBehindPlayer = unitsBehindPlayer;
        this.unitsAbovePlayer = unitsAbovePlayer;
        this._fsm = _fsm;
    }

    public void OnEnter()
    {
        _move.RemoveForces();

        pos[0] = Player_Movement.position + Vector3.up * unitsAbovePlayer;
        Vector3 dir = Player_Movement.position - _move.transform.position;
        pos[1] = Player_Movement.position + _move.transform.forward * unitsBehindPlayer ;
        _move._rb.constraints = RigidbodyConstraints.None;
               
    }

    public void OnExit()
    {
        count = 0;
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = Vector3.zero;
        }
        _move._rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    public void OnUpdate()
    {
        Move();
        if (Vector3.Distance(_move.transform.position,Player_Movement.position)<0.5f&&count==0)
            _explosion?.Invoke();
    }

    void Move()
    {
         _move.AddForce(_move.Seek(pos[count]));
        if (Vector3.Distance(pos[count], _move.transform.position) > 0.2f) return;
        
            if (count+1<pos.Length)
            {
               count++;
              _fsm.Debug("count++, paso al siguiente waypoint ");
            } 
                   
            else           
                _fsm.ChangeState("Pursuit");          
        
    }

    public void GizmoShow()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_move.transform.position, pos[count]);
        Gizmos.DrawLine(pos[0], pos[1]);
        for (int i = 0; i < pos.Length; i++)
        {
            Gizmos.DrawWireSphere(pos[0], 0.5f);
        }
        
    }

    
}
