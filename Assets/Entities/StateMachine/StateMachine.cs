using System.Collections.Generic;
using UnityEngine;

//T sera mi key
public class StateMachine<T> : MonoBehaviour
{
    IState _currentState;

    Dictionary<T, IState> _statesList = new Dictionary<T, IState>();

    IState _actualState;
  




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
            _actualState = _statesList[name];
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

