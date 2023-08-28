using UnityEngine;
using static EggEscapeModel;

public class EggState_Stunned : EggState<EggStates>
{
    float _actualTimeStunned;

    public EggState_Stunned(EggStateData data) : base(data) { }

    public override void OnEnter()
    {
        _agent.ManualMovement.MaxSpeed = 0;
        _agent.CancelMovement();
         _actualTimeStunned = _eggStats.stunTime;
    }

    public override void OnUpdate()
    {
        _actualTimeStunned -= Time.deltaTime;
        if (_actualTimeStunned <= 0)
        {
            if (_fov.IN_FOV(_eggStats.gameMode.playerPos))
                _fsm.ChangeState(EggStates.Escape);
            else
                _fsm.ChangeState(EggStates.Patrol);
        }

    }     



    public override void OnExit() { }  
}
