using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Worm_State : IState
{

    protected Enemy_Worm _worm;

    

    // Lo llama la state machine
    public Worm_State(Enemy_Worm worm) 
    {
        _worm = worm;
    }

    public virtual void GizmoShow() { }    

    public virtual void OnEnter() { }

    public virtual void OnExit() { }
    
    public virtual void OnUpdate() { }
}
