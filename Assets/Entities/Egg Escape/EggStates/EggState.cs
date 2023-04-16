using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EggEscapeModel;
using UnityEngine.AI;

public abstract class EggState : IState
{
    public enum EggStates
    {
        Patrol,
        Escape,
        Stunned,
        Kidnapped
    }
    public struct EggStateData
    {
        public EggStats _eggStats;
        public FOVAgent _fov;
        public NavMeshAgent _agent;
        public StateMachine<EggStates> _fsm;
    }

    public EggState(EggStateData data)
    {
        _eggStats = data._eggStats;
        _fov = data._fov;
        _agent = data._agent;
        _fsm = data._fsm;
    }
    public EggStats _eggStats;
    public FOVAgent _fov;
    public NavMeshAgent _agent;
    public StateMachine<EggStates> _fsm;

  
    
    public abstract void MakeDecision();
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();

    public virtual void GizmoState()
    {

    }
}
