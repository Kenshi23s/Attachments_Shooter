using System;
using System.Collections.Generic;
using UnityEngine;

//T sera mi key
public class StateMachine<T> 
{
    public T actualStateKey { get; private set; }
 

    public IState currentStateValue { get; private set; }

    Dictionary<T, IState> _statesList = new Dictionary<T, IState>();

    DebugableObject _debug;
    public void Initialize(DebugableObject _debug)
    {
        this._debug = _debug;
        this._debug.AddGizmoAction(StateGizmos);
    }
    public void CreateState(T name, IState state)
    {
        // si en mi diccionario no tengo esa llave 
        if (!_statesList.ContainsKey(name))
            //la creo
            _statesList.Add(name, state);
    }

    public void Execute()
    {
        currentStateValue.OnUpdate();
    }

    public void Debug(string msg)
    {
        _debug.Log("Estado "+ currentStateValue +" :"+ msg);
    }

    public void ChangeState(T name)
    {
        if (_statesList.ContainsKey(name))
        {
            var aux = currentStateValue;
            if (currentStateValue != null)
                currentStateValue.OnExit();

            actualStateKey = name;
            currentStateValue = _statesList[name];
            _debug.Log($"changed state : {aux} ===> {currentStateValue}");
            currentStateValue.OnEnter();
        }
        else
        {
            string debug = "";
            foreach (var item in _statesList)
            {
                debug += item.Key+", ";
            }
            _debug.Log($"el estado {name} no existe, en esta fsm solo existen los estados:"+debug);
        }
           
    }
    public void AnulateStates()
    {
        if (currentStateValue != null)
            currentStateValue.OnExit();
        actualStateKey = default;
        currentStateValue =  null;
    }
    public void StateGizmos() => currentStateValue?.GizmoShow();
}

