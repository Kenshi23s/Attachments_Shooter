using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IObjectSelectable : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler { }
/// <summary>
/// Hace que un GameObject con collider actue como un botton. Se necesita un EventSystem en escena, y un PhysicsRaycaster acoplado a la camara.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ButtonGameObject : MonoBehaviour, IObjectSelectable
{
 


    private bool isEnabled = true;
    public UnityEvent OnSelected, OnHighlighted, OnUnHighlighte, OnPressed;
    public UnityEvent OnEnable, OnDisable;



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEnabled) return;

     
        Debug.Log("POINTER ENTER");

        OnHighlighted?.Invoke();


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isEnabled) return;

    
        OnUnHighlighte?.Invoke();

        Debug.Log("POINTER EXIT");

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isEnabled) return;

        OnSelected?.Invoke();
        Debug.Log("POINTER CLICK");

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isEnabled) return;

      
        OnPressed?.Invoke();
        Debug.Log("POINTER DOWN");

    }


    // Use this function to disable/enable the button
    public void SetButtonState(bool newValue)
    {
        if (isEnabled == newValue) return;

        isEnabled = newValue;
        UnityEvent x = isEnabled ? OnEnable : OnDisable;
        x?.Invoke();
       

    }
}
