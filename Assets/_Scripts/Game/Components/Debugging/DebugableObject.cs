using System;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class DebugableObject : MonoBehaviour
{
    [SerializeField] public bool canDebug = true;
    
    public UnityEvent gizmoDraw;

    private void Awake() => enabled = false;

    public void AddGizmoAction(Action a) => gizmoDraw.AddListener(new UnityAction(a));

    void OnDrawGizmos()
    {
        if (!canDebug) return;
        gizmoDraw?.Invoke();
    }

    public void Log(string message)
    {
        if (!canDebug) return;
        Debug.Log(gameObject.name+": " +message);
    }

    public void WarningLog(string message)
    {
        if (!canDebug) return;
        Debug.LogWarning(gameObject.name + ": " + message);
    }

    public void ErrorLog(string message)
    {
        if (!canDebug) return;
        Debug.LogError(gameObject.name + ": " + message);
    }
}
