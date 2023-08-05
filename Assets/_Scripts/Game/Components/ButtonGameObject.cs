using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IObjectSelectable : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler { }
/// <summary>
/// Hace que un GameObject con collider actue como un botton. Se necesita un EventSystem en escena, y un PhysicsRaycaster acoplado a la camara.
/// </summary>
[RequireComponent(typeof(Collider), typeof(DebugableObject))]
[DisallowMultipleComponent]
public class ButtonGameObject : MonoBehaviour, IObjectSelectable
{
    [field:SerializeField] public bool IsEnabled { get; private set; } = true;
    public UnityEvent OnNormal, OnHighlighted, OnPressed, OnSelected;
    public UnityEvent OnEnable, OnDisable;

    DebugableObject _debugger;

    public void ClearAllEvents()
    {
        UnityEvent[] EventCol = 
        {
             OnNormal,OnHighlighted, OnPressed, OnSelected ,OnEnable, OnDisable
        };

        //foreach (var item in EventCol.Where(x => x.GetPersistentEventCount() > 0).Where(x => x != null))
        //{
        //    item.RemoveAllListeners();
        //}
    }
    private void Awake()
    {
        _debugger = GetComponent<DebugableObject>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
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

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!IsEnabled || eventData.selectedObject == gameObject) return;

        OnNormal?.Invoke();

        _debugger.Log("OBJECT STATE: NORMAL");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!IsEnabled || eventData.selectedObject == gameObject) return;

        EventSystem.current.SetSelectedGameObject(gameObject, eventData);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!IsEnabled || eventData.selectedObject == gameObject) return;

        OnPressed?.Invoke();
        _debugger.Log("OBJECT STATE: PRESSED");
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        OnSelected?.Invoke();
        _debugger.Log("OBJECT STATE: SELECTED");
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        OnNormal?.Invoke();
        _debugger.Log("OBJECT STATE: NORMAL");
    }

    // Usar esta funcion para activar o desactivar el boton.
    public void SetButtonState(bool enabled)
    {
        if (IsEnabled == enabled) return;

        IsEnabled = enabled;
        UnityEvent x = IsEnabled ? OnEnable : OnDisable;
        x?.Invoke();

        _debugger.Log("OBJECT STATE: " + (IsEnabled ? "ENABLED" : "DISABLED"));
    }
}
