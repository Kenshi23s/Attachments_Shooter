using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(DebugableObject))]
public class TickEventsManager : MonoSingleton<TickEventsManager>
{

    DebugableObject _debug;
    //lo podria haber hecho con eventos, pero no podria hacer estas preguntas de si es basado en tiempo O s
    List<TickEvent> _actionsSubscribed = new List<TickEvent>();
    bool coroutineRunning;

    protected override void SingletonAwake()
    {
        _debug = GetComponent<DebugableObject>();
        enabled = false;
    }
   
    public void AddAction(TickEvent x)
    {
        if (_actionsSubscribed.Contains(x)) return;

        if (x.isTimeBased) x.currentTime = x.timeStart;
        _actionsSubscribed.Add(x);
        if (!coroutineRunning) StartCoroutine(OverTimeCoroutine());
       
      
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
     
    }

    public event Action turnOffEvent;
    IEnumerator OverTimeCoroutine()
    {
        coroutineRunning = true;
        while (_actionsSubscribed.Count > 0)
        {
            List<TickEvent> toRemove = new List<TickEvent>();

            foreach (TickEvent actual in _actionsSubscribed)
            {
                actual.OnTickAction?.Invoke();
                if (!actual.isTimeBased) continue;

                if (ChangeTickTime(actual, 1))
                    toRemove.Add(actual);

            }

            foreach (var x in toRemove) _actionsSubscribed.Remove(x);
          
            yield return new WaitForSeconds(1);
        }
        coroutineRunning = false;
    }
    bool ChangeTickTime(TickEvent x,float time)
    {
        x.currentTime -= time;
        return 0 >= x.currentTime;
    }

}
public class TickEvent
{
    public Action OnTickAction;
    public bool isTimeBased;
    public float timeStart;
    public float currentTime;
}
