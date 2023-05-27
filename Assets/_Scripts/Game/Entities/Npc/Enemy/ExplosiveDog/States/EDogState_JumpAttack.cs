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
    Vector3 jumpWp;

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

        jumpWp = Player_Movement.position + Vector3.up * unitsAbovePlayer;
        state = Jump;
        _move._rb.constraints = RigidbodyConstraints.None;
               
    }

    public void OnExit()
    {        
        _move._rb.constraints = RigidbodyConstraints.FreezePositionY;
        state = null;
    }
    Action state;
    public void OnUpdate()=> state?.Invoke();

    void Jump()
    {          
        _move.AddForce(_move.Seek(jumpWp));
        if (Vector3.Distance(_move.transform.position, Player_Movement.position) < 0.5f)
            _explosion?.Invoke();
        if (Vector3.Distance(_move.transform.position, jumpWp)<0.5f)
        {
            state = Falling;
        }
    }
    void Falling()
    {
        Vector3 force = _move.transform.position + _move.transform.forward + Vector3.down;
        _move.AddForce(_move.Seek(force));
        if (Physics.Raycast(_move.transform.position,Vector3.down,1f))
        {
            _fsm.ChangeState("Pursuit");
        }
        else
        {
            _fsm.Debug("No choque con nada");
        }
    }

    public void GizmoShow()
    {
        //if (count >= jumpWp.Length) return;
        
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(_move.transform.position, jumpWp[count]);
        //Gizmos.DrawLine(jumpWp[0], jumpWp[1]);
        //for (int i = 0; i < jumpWp.Length; i++)
        //{
        //    Gizmos.DrawWireSphere(jumpWp[i], 0.5f);
        //}
        
    }

    
}
