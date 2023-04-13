using System.Collections.Generic;
using UnityEngine;

//T sera mi key
public class StateMachine<T> : MonoBehaviour
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
        _currentState.MakeDecision();
    }

    public void ChangeState(T name)
    {
        if (_statesList.ContainsKey(name))
        {
            _currentState = _statesList[name];
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = _statesList[name];
            _currentState.OnEnter();
            
        }

    }
    public void StateGizmos()
    {
        _currentState.GizmoState();
    }
}

