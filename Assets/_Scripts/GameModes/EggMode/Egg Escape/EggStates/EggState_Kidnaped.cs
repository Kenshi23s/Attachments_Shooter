using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EggEscapeModel;

public class EggState_Kidnaped : EggState<EggStates>
{ 
    float _actualKidnapTime;
    UnityEvent onGrab, onRelease;
    

    public EggState_Kidnaped(EggStateData data, UnityEvent onGrab, UnityEvent onRelease) : base(data) 
    {         
      this.onGrab = onGrab;
      this.onRelease = onRelease;         
    }

    public override void OnEnter()
    {
        onGrab?.Invoke();
        _actualKidnapTime = _eggStats.kidnapedTime;
        _agent.SetMaxSpeed(_eggStats.kidnapSpeed);
        _agent.Movement.maxForce = _eggStats.kidnapSpeed;
        _agent.CancelMovement();
       
    }
        
  
    public override void OnUpdate()
    {
        _actualKidnapTime -= Time.deltaTime;
        if (Vector3.Distance(_eggStats.gameMode.playerPos,_manual_Movement.transform.position) > _eggStats.kidnapFollowRadius)
        {
            _agent.SetDestination(_eggStats.gameMode.playerPos);
           
        }
        else
        {
            _manual_Movement.RemoveForces();
        }
      
       
        
        if (_actualKidnapTime <= 0)
            _fsm.ChangeState(EggStates.Escape);
    } 
  
   
  
    public override void OnExit() { onRelease?.Invoke(); }

    public override void GizmoShow()
    {
        base.GizmoShow();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_eggStats.gameMode.playerPos, _eggStats.kidnapFollowRadius);
    }
}
