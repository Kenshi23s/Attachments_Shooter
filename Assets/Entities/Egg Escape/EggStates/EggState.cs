using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EggEscapeModel;
using UnityEngine.AI;

public abstract class EggState : IState
{
    public enum States
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
        public StateMachine<States> _fsm;
        public Transform _transform;
    }

    public EggState(EggStateData data)
    {
        _eggStats = data._eggStats;
        _fov = data._fov;
        _agent = data._agent;
        _fsm = data._fsm;
        _transform = data._transform;
    }
    Transform _transform;
    public Vector3 myPos => _transform.position; 
    public EggStats _eggStats;
    public FOVAgent _fov;
    public NavMeshAgent _agent;
    public StateMachine<States> _fsm;

  
    
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();

    public virtual void GizmoShow()
    {

    }
}
