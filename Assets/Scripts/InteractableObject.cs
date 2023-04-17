using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct InteractableObjectData
{
    [SerializeField] public string _promptText;
    [SerializeField] public float _interactTimeNeeded;
    [SerializeField] public bool canInteract;
    [SerializeField] public UnityEvent _OnInteract;

    public InteractableObjectData(UnityEvent _OnInteract,string _promptText = "Interact", float _interactTimeNeeded = 2f, bool canInteract = true)
    {
        this._OnInteract = _OnInteract;
        this._promptText = _promptText;
        this._interactTimeNeeded = _interactTimeNeeded;
        this.canInteract = canInteract;
       
    }
}
[RequireComponent(typeof(SphereCollider))]
public class InteractableObject : MonoBehaviour
{
    [Header("InteractableObject")]
    [SerializeField] protected InteractableObjectData InteractData;
    [SerializeField] float _currentInteractionTime;
    [SerializeField] bool isTriyingToInteract;

    [Header("Canvas")]
    [SerializeField] Canvas _canvas;
    [SerializeField] Slider _sliderInteract;
    [SerializeField] Text _promptText;

    public float currentInteractionTime 
    {
      get => _currentInteractionTime; 
      set => _currentInteractionTime = Mathf.Clamp(value,0,InteractData._interactTimeNeeded); 
    }

    event Action<InteractableObjectData> onDataChange;

    event Action<float> WhileInteracting;

    event Action<float> While_NOT_Interacting;

    public void Init_InteractableObject(InteractableObjectData newData)
    {
        InteractData = newData;
        currentInteractionTime = 0;
        onDataChange?.Invoke(InteractData);
    }

    protected void DataChange() => onDataChange?.Invoke(InteractData);


    protected virtual void Awake()
    {
        currentInteractionTime = 0;
        this.GetComponent<SphereCollider>().isTrigger = true;
        SetUICalls();     
        //lo hago para que se hagan los llamados de "OnDataChange"
        Init_InteractableObject(InteractData);
        _canvas.gameObject.SetActive(false);
    } 

   



    public void Interact()
    {      
        
        //sumo cuanto tiempo mantuve pulsado el boton       
        currentInteractionTime += Time.deltaTime;
        WhileInteracting?.Invoke(currentInteractionTime);
        //si pase el tiempo indicado
        if (currentInteractionTime >= InteractData._interactTimeNeeded)
        {
            //ejecuto los eventos
            InteractData._OnInteract?.Invoke();
            InteractData.canInteract = false;
            
        }
        
        
    }
    private void LateUpdate()
    {
        //si no estoy interactuando bajo el tiempo q se mantuvo pulsado
        isTriyingToInteract = Input.GetKey(KeyCode.E);
        if (InteractData.canInteract && isTriyingToInteract)
                  Interact();        

        else if (!isTriyingToInteract && currentInteractionTime > 0)
        {
            currentInteractionTime = 0f;
            While_NOT_Interacting?.Invoke(currentInteractionTime);
        }


        if (_canvas.gameObject.activeInHierarchy)
        {
            Vector3 dir = _canvas.transform.position - Camera.main.transform.position;
            _canvas.transform.forward = new Vector3 (dir.x, 0, dir.z).normalized;
            
        }

        if (!InteractData.canInteract && Input.GetKeyUp(KeyCode.E))
        {
            InteractData.canInteract = true;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player_Movement player))
        {
            _canvas.gameObject.SetActive(true);
            
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player_Movement player))
        {
            _canvas.gameObject.SetActive(false);
        }
    }

 
    public void SetUICalls()
    {
        #region Slider
        onDataChange += (x) =>
        {
            _sliderInteract.maxValue = x._interactTimeNeeded;
            _sliderInteract.minValue = 0;
        };

        Action<float> ValueChange = (x) => _sliderInteract.value = x;

        WhileInteracting += ValueChange;
        While_NOT_Interacting += ValueChange;
        #endregion

        #region PromptText
        onDataChange += (x) => _promptText.text = x._promptText;
        #endregion
    }
}
