using FacundoColomboMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(EventTrigger))]
public abstract class InteractableCard : MonoBehaviour
{
   
    [field : SerializeField,Range(1f, 2f)]
    public float ScaleButtonBy { get; private set; }
    public EventTrigger EventTrigger { get; private set; }

    protected virtual void Awake()
    {
        EventTrigger = GetComponent<EventTrigger>();
        
        EventTrigger.CreateAndAddEvent(ScaleButton, EventTriggerType.PointerEnter);
        EventTrigger.CreateAndAddEvent(UnScaleButton, EventTriggerType.PointerExit);
    }

    public void ScaleButton()
    {
        transform.localScale = Vector3.one *  1.25f;
    }
    public void UnScaleButton()
    {
        transform.localScale = Vector3.one;
    }
}
public static class EventSystemExtension
{
    public static EventTrigger.Entry CreateEvent(Action method,EventTriggerType triggerType)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener(x => { method(); });
        return entry;
       
    }
    public static void CreateAndAddEvent(this EventTrigger trigger, Action method, EventTriggerType triggerType)
    {
        trigger.triggers.Add(CreateEvent(method,triggerType));
    }
}
