using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public interface IObjectSelectable : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler { }
/// <summary>
/// Hace que un GameObject con collider actue como un botton. Se necesita un EventSystem en escena, y un PhysicsRaycaster acoplado a la camara.
/// </summary>
[RequireComponent(typeof(Collider), typeof(DebugableObject))]
[DisallowMultipleComponent]
public class ButtonGameObject : MonoBehaviour, IObjectSelectable
{
    
    bool _interactable = true;
    public bool Interactable
    {
        get => _interactable;
        set => SetInteractable(value);
    }

    bool _pointerIsHovering;
    bool _pointerDownWhileInteractable;

    public UnityEvent OnNormal = new UnityEvent();
    public UnityEvent OnHighlighted = new UnityEvent();
    public UnityEvent OnPressed = new UnityEvent();
    public UnityEvent OnSelected = new UnityEvent();
    public UnityEvent OnEnabled = new UnityEvent();
    public UnityEvent OnDisabled = new UnityEvent();

    DebugableObject _debugger;


    public void ClearAllEvents()
    {
        UnityEvent[] EventCol =
        {
             OnNormal,OnHighlighted, OnPressed, OnSelected, OnEnabled, OnDisabled
        };

        foreach (var item in EventCol.Where(x => x.GetPersistentEventCount() > 0).Where(x => x != null))
        {
            item.RemoveAllListeners();
        }
    }

    private void Awake()
    {
        _debugger = GetComponent<DebugableObject>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _pointerIsHovering = true;

        if (!Interactable || eventData.selectedObject == gameObject) return;

        if (eventData.pointerPress)
        {
            OnPressed?.Invoke();
            _debugger.Log("BUTTON STATE: PRESSED");
        }
        else
        {
            OnHighlighted?.Invoke();
            _debugger.Log("BUTTON STATE: HIGHLIGHTED");
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _pointerIsHovering = false;
        if (!Interactable || eventData.selectedObject == gameObject) return;
        
        OnNormal?.Invoke();

        _debugger.Log("BUTTON STATE: NORMAL");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!_pointerDownWhileInteractable) return;

        _pointerDownWhileInteractable = false;

        if (!Interactable || eventData.selectedObject == gameObject) return;

        EventSystem.current.SetSelectedGameObject(gameObject, eventData);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!Interactable || eventData.selectedObject == gameObject) return;

        _pointerDownWhileInteractable = true;
        OnPressed?.Invoke();
        _debugger.Log("BUTTON STATE: PRESSED");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!_pointerIsHovering)
            _pointerDownWhileInteractable = false;
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        OnSelected?.Invoke();
        _debugger.Log("BUTTON STATE: SELECTED");
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        OnNormal?.Invoke();
        _debugger.Log("BUTTON STATE: NORMAL");
    }

    // Usar esta funcion para habilitar o deshabilitar el boton.
    void SetInteractable(bool value)
    {
        if (_interactable == value) return;
        _interactable = value;

        // Si se desactivo...
        if (!_interactable)
        {
            OnDisabled?.Invoke();

            // Asegurarnos de que si se desactivo mientras se estaba presionando,
            // no se tome el click cuando si se vuelve a activar.
            _pointerDownWhileInteractable = false;

            _debugger.Log("BUTTON STATE: DISABLED");
            return;
        }

        // Si se activo...
        OnEnabled?.Invoke();
        _debugger.Log("BUTTON STATE: ENABLED");

        // Chequear si el mouse esta por encima, y llamar los eventos indicados
        if (_pointerIsHovering)
        {
            OnHighlighted?.Invoke();
            _debugger.Log("BUTTON STATE: HIGHLIGHTED");
        }
        else
        {
            OnNormal?.Invoke();
            _debugger.Log("BUTTON STATE: NORMAL");
        }
    }


}
