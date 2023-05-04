using System.Collections.Generic;
using System;
using UnityEngine;

public class TickEventsManager : MonoSingleton<TickEventsManager>
{
    public struct TickEvents
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

    //lo podria haber hecho con eventos, pero no podria hacer estas preguntas de si es basado en tiempo O s
    List<TickEvents> _actionsSubscribed = new List<TickEvents>();
  
    
    protected override void ArtificialAwake() => enabled = false;
   
    public void AddAction(TickEvents action)
    {
        if (_actionsSubscribed.Contains(action)) return;

        _actionsSubscribed.Add(action); enabled = true;
        if (action.isTimeBased) action.currentTime = action.timeStart;
    }

    public void RemoveAction(TickEvents action)
    {
        _actionsSubscribed.Remove(action);
        if (_actionsSubscribed.Count == 0) enabled = false; //para q el update deje de hacer tick
    }

    public void AddTimeToEvent(TickEvents action, float timeToAdd)
    {
        if (_actionsSubscribed.Contains(action) && action.isTimeBased)
            action.currentTime += timeToAdd;
        else
        {
            Func<bool, string> analizeBool = (x) => x ? "SI" : "No";

            string debugSubscription = analizeBool(_actionsSubscribed.Contains(action));

            string debugTimeBased = analizeBool(action.isTimeBased);

            Debug.LogWarning("El evento "  + action.ToString() +  debugSubscription + "esta en mi lista de eventos, " + debugTimeBased +" es basado en el tiempo");
          
        }    
    }

    private void Update()
    {
        //solo si on enabled esta en true
        foreach (TickEvents actual in _actionsSubscribed)
        {
            actual.OnTickAction();
            if (actual.TimesUp(Time.deltaTime)) RemoveAction(actual);            
        }
    }
}
