using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FacundoColomboMethods;

public class EDogState_Idle : IState
{
    IA_Movement _agent;
    StateMachine<string> _fsm;

    public EDogState_Idle(IA_Movement agent, StateMachine<string> fsm)
    {
        _agent = agent;
        _fsm = fsm;
    }

    public void OnEnter()
    {
        GameManager.instance.HelpStartCoroutine(StayIdle);
    }

    IEnumerator StayIdle()
    {
        while (true)
        {
            //Player_Movement z = _agent.transform.position.GetItemsOFTypeAround<Player_Movement>(_agent._fov.viewRadius)
            //    .Where(x => _agent.transform.position.InLineOffSight(x.transform.position,IA_Manager.instance.wall_Mask)).ToList().First();

            //.OfType<Player_Movement>()
            //.Where(x => _agent.transform.position.InLineOffSight(x.transform.position, IA_Manager.instance.wall_Mask))
            //.First();

            if (_agent._fov.inFOV(Player_Movement.position))
            {
                _fsm.Debug("Veo al player"); _fsm.ChangeState("Pursuit");
                break;
            }
            else
            {
               _fsm.Debug("El player no esta cerca");
            }

              
           
             

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
