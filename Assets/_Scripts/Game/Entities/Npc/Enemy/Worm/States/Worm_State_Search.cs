using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_Worm;

public class Worm_State_Search : Worm_State<EWormStates>
{
    public Worm_State_Search(Enemy_Worm worm) : base(worm)
    {
    }

    Vector3 _enterPos;

    public override void OnEnter()
    {
        _enterPos = _worm.transform.position;
        _worm.AI_move.SetDestination(Player_Movement.position);

        // Si el gusano llega a su destino sin encontrar al jugador, que regrese a su posicion original
        _worm.AI_move.OnDestinationReached += ReturnToEnterPosition;
        _worm.anim.SetFloat("Speed", 1);

    }

    public override void OnUpdate()
    {
        // Mientras el jugador no este a la vista, permanecer en este estado
        if (_worm.AI_move.FOV.IN_FOV(Player_Movement.position))
            _fsm.ChangeState(EWormStates.Attack);
    }

    void ReturnToEnterPosition() 
    {
        _worm.AI_move.OnDestinationReached -= ReturnToEnterPosition;
        _worm.AI_move.SetDestination(_enterPos);
        _worm.AI_move.OnDestinationReached += ReturnToIdle;
    }

    void ReturnToIdle() 
    {
        //_worm.AI_move.OnDestinationReached -= ReturnToIdle; Lo remuevo porque si se va a remover en el exit no hace falta removerlo aca
        _fsm.ChangeState(EWormStates.Idle);
    }

    public override void OnExit() 
    {
        // Por las dudas remover estos metodos al salir del estado.
        _worm.AI_move.OnDestinationReached -= ReturnToEnterPosition;
        _worm.AI_move.OnDestinationReached -= ReturnToIdle;
        _worm.AI_move.CancelMovement();
    }
  
}
