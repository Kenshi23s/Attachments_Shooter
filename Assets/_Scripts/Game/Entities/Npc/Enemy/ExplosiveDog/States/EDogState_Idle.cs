using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FacundoColomboMethods;
using System;

using static E_ExplosiveDog;

public class EDogState_Idle : IState<EDogStates>
{
    AI_Movement _agent;
    StateMachine<EDogStates> _fsm;
    LifeComponent myLifeComponent;
    Action<int> callBackChange;

    public EDogState_Idle(AI_Movement agent, StateMachine<EDogStates> fsm,LifeComponent myLifeComponent)
    {
        _agent = agent;
        _fsm = fsm;
        this.myLifeComponent = myLifeComponent;
        callBackChange = (x) => { _fsm.ChangeState(EDogStates.PURSUIT); GameManager.instance.HelpStopCoroutine(StayIdle); ; };
       


    }

    public void OnEnter()
    {
        myLifeComponent.OnTakeDamage += callBackChange;
        GameManager.instance.HelpStartCoroutine(StayIdle);
       
    }

    IEnumerator StayIdle()
    {
        while (_fsm.actualStateKey == EDogStates.IDLE)
        {
            if (_fsm.actualStateKey != EDogStates.IDLE) break;



            //Player_Movement z = _agent.transform.position.GetItemsOFTypeAround<Player_Movement>(_agent._fov.viewRadius)
            //    .Where(x => _agent.transform.position.InLineOffSight(x.transform.position,IA_Manager.instance.wall_Mask)).ToList().First();

            //.OfType<Player_Movement>()
            //.Where(x => _agent.transform.position.InLineOffSight(x.transform.position, IA_Manager.instance.wall_Mask))
            //.First();
            //buscar alguna manera que no sea calculando la posicion
            if (_agent.FOV.IN_FOV(Player_Movement.position))
            {
                _fsm.Debug("Veo al player"); _fsm.ChangeState(EDogStates.PURSUIT);
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
        myLifeComponent.OnTakeDamage -= callBackChange;
    }

    

    public void GizmoShow()
    {

    }

    public void SetStateMachine(StateMachine<EDogStates> fsm)
    {
    }
}
