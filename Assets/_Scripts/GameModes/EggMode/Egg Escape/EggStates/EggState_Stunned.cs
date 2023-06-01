using UnityEngine;

public class EggState_Stunned : EggState
{
    float _actualTimeStunned;

    public EggState_Stunned(EggStateData data) : base(data) { }

    public override void OnEnter()
    {
         _agent.SetMaxSpeed(0);
         _actualTimeStunned = _eggStats.stunTime;
    }

    public override void OnUpdate()
    {
        _actualTimeStunned -= Time.deltaTime;
        if (_actualTimeStunned <= 0)
        {
            if (_fov.inFOV(_eggStats.gameMode.playerPos))
                _fsm.ChangeState(States.Escape);
            else
                _fsm.ChangeState(States.Patrol);
        }

    }     



    public override void OnExit() { }  
}
