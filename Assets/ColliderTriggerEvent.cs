using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderTriggerEvent : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnStay;
    public UnityEvent OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.TryGetComponent(out Player_Handler player))
            return;
        Debug.LogWarning("Enter"+ gameObject.name);
        OnEnter.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.root.TryGetComponent(out Player_Handler player))
            return;

        OnEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.TryGetComponent(out Player_Handler player))
            return;

        OnExit.Invoke();
    }

    public void DestroyGameObject(GameObject gameObject) 
    { 
        Destroy(gameObject);
    }
}
