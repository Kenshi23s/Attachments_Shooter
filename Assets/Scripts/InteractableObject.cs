using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct InteractableObjectData
{
    [SerializeField] public string _promptText = "Interact";
    [SerializeField] public float _interactTimeNeeded = 2f;
    [SerializeField] public bool canInteract;
    [SerializeField] public UnityEvent _OnInteract;

    public InteractableObjectData(UnityEvent _OnInteract, string _promptText = "Interact", float _interactTimeNeeded = 2f)
    {
        this._OnInteract = _OnInteract;
        this._promptText = _promptText;
        this._interactTimeNeeded = _interactTimeNeeded;
    }
}
public class InteractableObject : MonoBehaviour
{
    [SerializeField]InteractableObjectData data;
    [SerializeField] float _currentInteractionTime;
    [SerializeField] bool isInteracting;

    public float currentInteractionTime 
    {
      get => _currentInteractionTime; 
      set => _currentInteractionTime = Mathf.Clamp(value,0,data._interactTimeNeeded); 
    }
        

    private void Awake()
    {
        currentInteractionTime = 0;
    }

    public void Init_InteractableObject(InteractableObjectData newData) => data = newData;
   
    public bool Interact()
    {
        if (data.canInteract)
        {
            currentInteractionTime += Time.deltaTime;

            if (currentInteractionTime >= data._interactTimeNeeded)
            {
                data._OnInteract?.Invoke();
            }
            return true;
        }

        return false;
        
    }
    private void LateUpdate()
    {
        if (!isInteracting)
        {
            currentInteractionTime -= Time.deltaTime;
        }
    }
}
