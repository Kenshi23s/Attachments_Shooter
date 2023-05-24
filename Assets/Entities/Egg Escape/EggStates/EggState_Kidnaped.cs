using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EggEscapeModel;

public class EggState_Kidnaped : EggState
{ 
    float _actualKidnapTime;
    Action onGrab;
    Action onRelease;

    public EggState_Kidnaped(EggStateData data,Action onGrab,Action onRelease) : base(data) 
    {         
      this.onGrab = onGrab;
      this.onRelease = onRelease;         
    }

    public override void OnEnter()
    {
        onGrab?.Invoke();
        _actualKidnapTime = _eggStats.kidnapedTime;
        _agent.SetMaxSpeed(_eggStats.kidnapSpeed);
        _agent.ClearPath();
       
    }
        
  
    public override void OnUpdate()
    {
        _actualKidnapTime -= Time.deltaTime;
        if (Vector3.Distance(_eggStats.gameMode.playerPos,_manual_Movement.transform.position)>5f)
        {
            Vector3 force = _manual_Movement.Seek(_eggStats.gameMode.playerPos);
            _manual_Movement.AddForce(force);
        }
        else
        {
            _manual_Movement.RemoveForces();
        }
      
       
        
        if (_actualKidnapTime <= 0)
            _fsm.ChangeState(States.Escape);
    } 
  
   
  
    public override void OnExit() { onRelease?.Invoke(); }

    public override void GizmoShow()
    {
        base.GizmoShow();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_eggStats.gameMode.playerPos, _eggStats.kidnapFollowRadius);
    }
}
