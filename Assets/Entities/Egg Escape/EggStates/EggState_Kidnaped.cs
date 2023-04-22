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
    }
        
  
    public override void OnUpdate()
    {
        _actualKidnapTime -= Time.deltaTime;
       
        if (Vector3.Distance(myPos, _eggStats.gameMode.playerPos) > _eggStats.kidnapFollowRadius)        
            _agent.SetDestination(_eggStats.gameMode.playerPos);
        
        if (_actualKidnapTime <= 0)
            _fsm.ChangeState(States.Escape);
    } 
  
   
  
    public override void OnExit() { onRelease?.Invoke(); }
}
