using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IObjectSelectable : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler { }
/// <summary>
/// Hace que un GameObject con collider actue como un botton. Se necesita un EventSystem en escena, y un PhysicsRaycaster acoplado a la camara.
/// </summary>
[RequireComponent(typeof(Collider), typeof(DebugableObject))]
public class ButtonGameObject : MonoBehaviour, IObjectSelectable
{
    public bool IsEnabled { get; private set; } = true;
    public UnityEvent OnSelected, OnHighlighted, OnUnhighlighted, OnPressed;
    public UnityEvent OnEnable, OnDisable;

    public DebugableObject _debugger;

    private void Awake()
    {
        _debugger = GetComponent<DebugableObject>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEnabled || eventData.selectedObject == gameObject) return;

        if (eventData.pointerPress)
        {
            OnPressed?.Invoke();
            _debugger.Log("OBJECT STATE: PRESSED");
        }
        else
        {
            OnHighlighted?.Invoke();
            _debugger.Log("OBJECT STATE: HIGHLIGHTED");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsEnabled || eventData.selectedObject == gameObject) return;

        OnUnhighlighted?.Invoke();

        _debugger.Log("OBJECT STATE: NORMAL");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEnabled || eventData.selectedObject == gameObject) return;

        EventSystem.current.SetSelectedGameObject(gameObject, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsEnabled || eventData.selectedObject == gameObject) return;

        OnPressed?.Invoke();
        _debugger.Log("OBJECT STATE: PRESSED");
    }


    // Use this function to disable/enable the button
    public void SetButtonState(bool newValue)
    {
        if (IsEnabled == newValue) return;

        IsEnabled = newValue;
        UnityEvent x = IsEnabled ? OnEnable : OnDisable;
        x?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelected?.Invoke();
        _debugger.Log("OBJECT STATE: SELECTED");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnUnhighlighted?.Invoke();
        Debug.Log("OBJECT STATE: NORMAL");
    }
}
