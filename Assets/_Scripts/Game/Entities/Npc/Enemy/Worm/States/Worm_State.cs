using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Worm_State<T> : IState<T>
{

    protected Enemy_Worm _worm;

    protected StateMachine<T> _fsm;

    // Lo llama la state machine
    public Worm_State(Enemy_Worm worm) 
    {
        _worm = worm;
    }

    public void SetStateMachine(StateMachine<T> fsm) 
    {
        // Si ya tiene asignada una state machine 
        if (_fsm != null)
        {
            Debug.LogWarning($"[Custom Msg] - {this} ya pertenece a una maquina de estado.");
            return;
        }

        _fsm = fsm;
    }

    public virtual void GizmoShow() { }    

    public virtual void OnEnter() { }

    public virtual void OnExit() { }
    
    public virtual void OnUpdate() { }
}
