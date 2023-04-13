using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct InteractableObjectData
{
    [SerializeField] public string _promptText;
    [SerializeField] public float _interactTimeNeeded;
    [SerializeField] public bool canInteract;
    [SerializeField] public UnityEvent _OnInteract;

    public InteractableObjectData(UnityEvent _OnInteract,bool canInteract ,string _promptText = "Interact", float _interactTimeNeeded = 2f)
    {
        this._OnInteract = _OnInteract;
        this._promptText = _promptText;
        this._interactTimeNeeded = _interactTimeNeeded;
        this.canInteract = canInteract;
    }
}
public class InteractableObject : MonoBehaviour
{
    [Header("InteractableObject")]
    [SerializeField] InteractableObjectData data;
    [SerializeField] float _currentInteractionTime;
    [SerializeField] bool isInteracting;

    public float currentInteractionTime 
    {
      get => _currentInteractionTime; 
      set => _currentInteractionTime = Mathf.Clamp(value,0,data._interactTimeNeeded); 
    }
        

    private void Awake() => currentInteractionTime = 0;

    public void Init_InteractableObject(InteractableObjectData newData) => data = newData;
   
    public bool Interact()
    {
        //si puedo interactuar
        if (data.canInteract)
        {
            //sumo cuanto tiempo mantuve pulsado el boton
            currentInteractionTime += Time.deltaTime;
            isInteracting= true;
            //si pase el tiempo indicado
            if (currentInteractionTime >= data._interactTimeNeeded)
            {
                //ejecuto los eventos
                data._OnInteract?.Invoke();
            }
            return true;
        }
        isInteracting= false;
        return false;
        
    }
    private void LateUpdate()
    {
        //si no estoy interactuando bajo el tiempo q se mantuvo pulsado
        if (!isInteracting)
        {
            currentInteractionTime -= Time.deltaTime;
        }
    }
}
