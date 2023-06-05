using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDogState_JumpAttack : IState
{

    Action _explosion;
    Action state;
    Physics_Movement _move;

    float unitsAbovePlayer=2.5f;
    StateMachine<string> _fsm;


    Vector3 jumpWp = Vector3.zero;

    public EDogState_JumpAttack(Action _explosion, Physics_Movement _move, float unitsAbovePlayer, StateMachine<string> _fsm)
    {
        this._explosion = _explosion;
        this._move = _move;     
        this.unitsAbovePlayer = unitsAbovePlayer;
        this._fsm = _fsm;
    }

    public void OnEnter()
    {
        jumpWp = Player_Movement.position + Vector3.up * unitsAbovePlayer;
        state = Jump;
        //_move._rb.constraints = RigidbodyConstraints.None;
               
    }

    public void OnExit()
    {        
        //_move._rb.constraints = RigidbodyConstraints.FreezePositionY;
        state = null;
        _move.RemoveForces();
    }


    public void OnUpdate()
    {
        state?.Invoke();
        if (Vector3.Distance(_move.transform.position, Player_Movement.position) < 2f)
            _explosion?.Invoke();
    } 

    void Jump()
    {          
        _move.AddImpulse(Vector3.up * unitsAbovePlayer);      
            state = Falling;     
    }
    void Falling()
    {
        //jumpForce = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
        if (!_move.isFalling) return;
       
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
