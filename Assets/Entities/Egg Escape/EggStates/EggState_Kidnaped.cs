using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EggEscapeModel;

public class EggState_Kidnaped : EggState
{ 
    float _actualKidnapTime;

    public EggState_Kidnaped(EggStateData data) : base(data) { }

    public override void OnEnter() => _actualKidnapTime = _eggStats.kidnapedTime;
  
    public override void OnUpdate() => _actualKidnapTime -= Time.deltaTime;
  
    public override void MakeDecision()
    {
        if (_actualKidnapTime<=0)        
            _fsm.ChangeState(EggStates.Escape);  
    }
  
    public override void OnExit() { }
}
