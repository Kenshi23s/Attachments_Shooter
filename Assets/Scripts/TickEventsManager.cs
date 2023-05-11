using System.Collections.Generic;
using System;
using UnityEngine;
using System.Diagnostics;

[RequireComponent(typeof(DebugableObject))]
public class TickEventsManager : MonoSingleton<TickEventsManager>
{
    public struct TickEvent
    {
        public Action OnTickAction;
        public bool isTimeBased;
        public float timeStart;
        public float currentTime;

        public bool TimesUp(float tick)
        {
            if (!isTimeBased) return false;

            currentTime -= tick;

            return currentTime <= 0;

        }
    }

    DebugableObject _debug;
    //lo podria haber hecho con eventos, pero no podria hacer estas preguntas de si es basado en tiempo O s
    List<TickEvent> _actionsSubscribed = new List<TickEvent>();

    protected override void ArtificialAwake()
    {
        _debug = GetComponent<DebugableObject>();
        enabled = false;
    }
   
    public void AddAction(TickEvent action)
    {
        if (_actionsSubscribed.Contains(action)) return;

        _actionsSubscribed.Add(action); enabled = true;
        if (action.isTimeBased) action.currentTime = action.timeStart;
    }

    public void RemoveAction(TickEvent action)
    {
        _actionsSubscribed.Remove(action);
        if (_actionsSubscribed.Count == 0) enabled = false; //para q el update deje de hacer tick
    }

    public void AddTimeToEvent(TickEvent action, float timeToAdd)
    {
        if (_actionsSubscribed.Contains(action) && action.isTimeBased)
            action.currentTime += timeToAdd;
        else
        {
            Func<bool, string> analizeBool = (x) => x ? "SI" : "No";

            string debugSubscription = analizeBool?.Invoke(_actionsSubscribed.Contains(action));

            string debugTimeBased = analizeBool?.Invoke(action.isTimeBased);

            _debug.WarningLog("El evento "  + action.ToString() +  debugSubscription + "esta en mi lista de eventos, " 
                              + debugTimeBased +" es basado en el tiempo");        
        }    
    }

    private void Update()
    {
        //solo si on enabled esta en true
        foreach (TickEvent actual in _actionsSubscribed)
        {
            actual.OnTickAction();
            if (actual.TimesUp(Time.deltaTime)) RemoveAction(actual);            
        }
    }
}
