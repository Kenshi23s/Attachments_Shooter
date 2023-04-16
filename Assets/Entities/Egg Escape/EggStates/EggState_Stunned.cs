using UnityEngine;

public class EggState_Stunned : EggState
{
    float _actualTimeStunned;

    public EggState_Stunned(EggStateData data) : base(data) { }

    public override void OnEnter()
    {
         _agent.speed = 0;
         _actualTimeStunned = _eggStats.stunTime;
    }

    public override void OnUpdate() => _actualTimeStunned -= Time.deltaTime;      

    public override void MakeDecision()
    {
        if (_actualTimeStunned<=0)
        {
            if (_fov.inFOV(_eggStats.gameMode.playerPos))       
                _fsm.ChangeState(EggStates.Escape); 
            else
                _fsm.ChangeState(EggStates.Patrol);
        }      
    }

    public override void OnExit() { }  
}
