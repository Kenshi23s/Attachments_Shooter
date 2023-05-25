using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FacundoColomboMethods;

public class EDogState_Idle : IState
{
    IA_Movement _agent;
    StateMachine<string> _fsm;
    
    public void OnEnter()
    {
        GameManager.instance.HelpStartCoroutine(StayIdle);
    }

    IEnumerator StayIdle()
    {
        while (true)
        {
            if (Physics.OverlapSphere(_agent.transform.position, _agent._fov.viewRadius)
                .OfType<Player_Movement>()
                .Select(x => x.transform.position)
                .Where(x => _agent.transform.position.InLineOffSight(x,IA_Manager.instance.wall_Mask)).Any())
                _fsm.ChangeState("Pursuit");
            yield return new WaitForSeconds(1);
        }     
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        GameManager.instance.HelpStopCoroutine(StayIdle);
    }

    public void GizmoShow()
    {

    }

    

}
