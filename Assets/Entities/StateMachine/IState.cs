using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    void SetStateMachine(StateMachine<T> fsm);
    void GizmoShow();
}
