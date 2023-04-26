using System;
using System.Collections.Generic;
using UnityEngine;

//T sera mi key
public class StateMachine<T> 
{
    public T actualState=> _actualState;
    public T _actualState;

    IState _currentState;

    Dictionary<T, IState> _statesList = new Dictionary<T, IState>();

    public void CreateState(T name, IState state)
    {
        // si en mi diccionario no tengo esa llave 
        if (!_statesList.ContainsKey(name))
            //la creo
            _statesList.Add(name, state);
    }

    public void Execute()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(T name)
    {
        if (_statesList.ContainsKey(name))
        {
            var aux = _currentState;
            if (_currentState != null)
                _currentState.OnExit();

            _actualState=name;
            _currentState = _statesList[name];
            Debug.Log($"changed state : {aux} ===> {_currentState}");            
            _currentState.OnEnter();
        }
        else        
            Debug.Log($"el estado { name } no existe");  
    }
    public void StateGizmos() => _currentState?.GizmoShow();
}

