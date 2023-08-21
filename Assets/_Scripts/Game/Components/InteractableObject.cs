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
[RequireComponent(typeof(DebugableObject))]
public class InteractableObject : MonoBehaviour
{
    [Header("InteractableObject")]
    [SerializeField] protected InteractableObjectData InteractData;
    [SerializeField] float _currentInteractionTime;
    [SerializeField] bool _isTriyingToInteract;
    [SerializeField] bool _cycleInteract;

    [Header("Canvas")]
    [SerializeField] TextAndFiller _canvas;
    

    [SerializeField]
    protected DebugableObject _debug;

    public float currentInteractionTime 
    {
      get => _currentInteractionTime; 
      set => _currentInteractionTime = Mathf.Clamp(value,0,InteractData._interactTimeNeeded); 
    }

    event Action<InteractableObjectData> onDataChange;

    event Action<float> WhileInteracting;

    event Action<float> While_NOT_Interacting;

    protected Action UpdateCheck;

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
        _debug=GetComponent<DebugableObject>();
        GetComponent<SphereCollider>().isTrigger = true;

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
        if (currentInteractionTime >= InteractData._interactTimeNeeded && _cycleInteract)
        {
            //ejecuto los eventos
            _cycleInteract = false;
            InteractData._OnInteract.Invoke();                  
        }

        //prende el slider
        SetVisibleSlider(true);
    }

    private void LateUpdate()
    {
        //si no estoy interactuando bajo el tiempo q se mantuvo pulsado
        _isTriyingToInteract = Input.GetKey(KeyCode.E);

        if (InteractData.canInteract && _isTriyingToInteract && _canvas.isActiveAndEnabled)
        {
            Interact(); _canvas.TurnSliderTextOff();
        }
        else
        {
            _canvas.TurnSliderTextOn();
        }
       
       
                   

        if (!_isTriyingToInteract && currentInteractionTime > 0)
        {
            currentInteractionTime = 0f;
            SetVisibleSlider(false);
            While_NOT_Interacting?.Invoke(currentInteractionTime);
        }

        if (!_cycleInteract && _isTriyingToInteract)
            _cycleInteract = true;

        if (_canvas.gameObject.activeInHierarchy)
        {
            Vector3 dir = _canvas.transform.position - Camera.main.transform.position;
            _canvas.transform.forward = new Vector3 (dir.x, 0, dir.z).normalized;
            
        }

      


        UpdateCheck?.Invoke();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player_Handler player))        
            _canvas.gameObject.SetActive(true);
        
     
        
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player_Handler player))       
            _canvas.gameObject.SetActive(false);
        
    
        
    }

 
    public void SetUICalls()
    {
        Action<float> ValueChange = (x) => _canvas.SetSliderValue(currentInteractionTime / InteractData._interactTimeNeeded);

        WhileInteracting += ValueChange;
        While_NOT_Interacting += ValueChange;
    

        #region PromptText
        onDataChange += (x) => _canvas.SetText(x._promptText);
        #endregion
    }

    public void SetVisibleSlider(bool mybool)
    {
        _canvas.gameObject.SetActive(mybool);
    }

    public void OnDesactivate()
    {
        _currentInteractionTime = 0f;        
        this.gameObject.SetActive(false);
    }

    public void OnActivate()
    {
        _cycleInteract = true;
        this.gameObject.SetActive(true);
    }
}
