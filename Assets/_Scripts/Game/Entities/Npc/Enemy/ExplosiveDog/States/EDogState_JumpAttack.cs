using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static E_ExplosiveDog;

public class EDogState_JumpAttack : IState<EDogStates>
{

    Action _explosion;
    Action state;
    Physics_Movement _move;

    float unitsAbovePlayer=2.5f,_triggerRadius;
    StateMachine<EDogStates> _fsm;


    Vector3 jumpWp = Vector3.zero;

    public event Action onEnter;
    public event Action onExit;


    public EDogState_JumpAttack(float triggerRadius,Action _explosion, Physics_Movement _move, float unitsAbovePlayer, StateMachine<EDogStates> _fsm)
    {
        this._explosion = _explosion;
        this._move = _move;     
        this.unitsAbovePlayer = unitsAbovePlayer;
        this._fsm = _fsm;
        _triggerRadius = triggerRadius;
    }

    public void OnEnter()
    {
        jumpWp = Player_Movement.position + Vector3.up * unitsAbovePlayer;
        state = Jump;
        onEnter?.Invoke();
        //_move._rb.constraints = RigidbodyConstraints.None;

    }

    public void OnExit()
    {
        //_move._rb.constraints = RigidbodyConstraints.FreezePositionY;
        onExit?.Invoke();
        state = null;
        _move.RemoveForces();
    }


    public void OnUpdate()
    {
        state?.Invoke();
        if (Vector3.Distance(_move.transform.position, Player_Movement.position) < _triggerRadius)
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
            _fsm.ChangeState(EDogStates.PURSUIT);      
        else        
            _fsm.Debug("No choque con nada");
        
    }

    public void GizmoShow()
    {
     
        
    }

    public void SetStateMachine(StateMachine<EDogStates> fsm)
    {
    }
}
