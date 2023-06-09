using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EggEscapeModel;
using UnityEngine.AI;

public abstract class EggState<T> : IState<T>
{
    

    public struct EggStateData
    {
        public EggStats _eggStats;
        public FOVAgent _fov;
        public AI_Movement _agent;
        public StateMachine<EggStates> _fsm;
        public Physics_Movement manual_Movement;
    }

    public EggState(EggStateData data)
    {
        _eggStats = data._eggStats;
        _fov = data._fov;
        _agent = data._agent;
        _fsm = data._fsm;
        _manual_Movement = data.manual_Movement;
    }

    //podria haber hecho un "EggData" propio de la clase y usarlo
    //pero quedaba muy ilegible el codigo, pq uso estas variables muy seguido
    //y siempre ponia data."Variable" 
    //EggStateData data;
    public Physics_Movement _manual_Movement;
    public Vector3 myPos => _manual_Movement.transform.position; 
    public EggStats _eggStats;
    public FOVAgent _fov;
    public AI_Movement _agent;
    public StateMachine<EggStates> _fsm;
    
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }

    public virtual void GizmoShow()
    {

    }

    public void SetStateMachine(StateMachine<T> fsm)
    {
        
    }
}
