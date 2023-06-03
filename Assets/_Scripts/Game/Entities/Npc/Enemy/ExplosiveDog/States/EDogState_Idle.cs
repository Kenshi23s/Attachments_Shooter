using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FacundoColomboMethods;

public class EDogState_Idle : IState
{
    AI_Movement _agent;
    StateMachine<string> _fsm;
    LifeComponent myLifeComponent;

    public EDogState_Idle(AI_Movement agent, StateMachine<string> fsm,LifeComponent myLifeComponent)
    {
        _agent = agent;
        _fsm = fsm;
        this.myLifeComponent = myLifeComponent;


    }

    public void OnEnter()
    {
        GameManager.instance.HelpStartCoroutine(StayIdle);
        myLifeComponent.OnTakeDamage += (x) => Change();
    }

    void Change()
    {
        _fsm.ChangeState("Pursuit");
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
            //buscar alguna manera que no sea calculando la posicion
            if (_agent.FOV.inFOV(Player_Movement.position))
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
        myLifeComponent.OnTakeDamage -= (x) => Change();
    }

    public void GizmoShow()
    {

    }

    

}
