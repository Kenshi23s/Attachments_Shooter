using System;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class DebugableObject : MonoBehaviour
{
    [SerializeField] public bool canDebug = true;
    
    public UnityEvent gizmoDraw;

    public void AddGizmoAction(Action a)
    {
        gizmoDraw.AddListener(new UnityAction(a));
    }

    void OnDrawGizmos()
    {
        if (!canDebug) return;

        gizmoDraw?.Invoke();
    }

    public void Log(string message)
    {
        if (!canDebug) return;

        Debug.Log(message);
    }

    public void WarningLog(string message)
    {
        if (!canDebug) return;

        Debug.LogWarning(message);
    }

    public void ErrorLog(string message)
    {
        if (!canDebug) return;

        Debug.LogError(message);
    }
}
