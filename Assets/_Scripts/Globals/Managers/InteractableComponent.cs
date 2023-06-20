using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(DebugableObject))]
public class InteractableComponent : MonoBehaviour, IInteractable
{
    
    [SerializeField] float interactDistance;
    public float interactTimeNeeded, currentInteractTime;
    float _checkInteractTime;
    [SerializeField] Collider _interactableCollider;

    public UnityEvent onFocus, onUnFocus, OnInteract, onTryingToInteract, OnInteractAbort;

    public List<Func<bool>> interactConditions = new List<Func<bool>>();

    DebugableObject _debug;

    private void Awake()
    {
        interactConditions.Add(()=>true);
    }
    private void Start()
    {
        InteractablesManager.instance.AddInteractableObject(this);
      

        //_interactableCollider=GetComponent<Collider>();
        _debug = GetComponent<DebugableObject>();
        OnInteract.AddListener(() => _debug.Log("Interactuan conmigo"));
        onFocus.AddListener(() => _debug.Log("Me focusean"));
        onUnFocus.AddListener(() => _debug.Log("Me dejaron de focusear conmigo"));
        _debug.AddGizmoAction(DrawRadius);
    }

    private void OnDestroy()
    {
        InteractablesManager.instance.RemoveInteractableObject(this);

        onFocus.RemoveAllListeners();
        onUnFocus.RemoveAllListeners();
        OnInteract.RemoveAllListeners();
        onTryingToInteract.RemoveAllListeners();
        OnInteractAbort.RemoveAllListeners();
    }


    public void NoMoreInteraction()
    {
        InteractablesManager.instance.RemoveInteractableObject(this);

        onFocus.RemoveAllListeners();
        onUnFocus.RemoveAllListeners();
        OnInteract.RemoveAllListeners();
        onTryingToInteract.RemoveAllListeners();
        OnInteractAbort.RemoveAllListeners();
    }

    // En vez de estar constantemente agregando y removiendo el interactuable, tal vez sea mejor que la interfaz tenga un metodo 'isActive'
    // y que mediante esta se determine si se debe interactuar o no con ella.

    
    public void Interact()
    {

        foreach (var item in interactConditions)
        {
            if (!item.Invoke())
            {
                _debug.Log("Las condiciones dieron falso, no se puede interactuar");
                return;
            }
        }
      
      
        _checkInteractTime = currentInteractTime += Time.deltaTime;
        
        onTryingToInteract?.Invoke();
        if (currentInteractTime>=interactTimeNeeded)
        {
            OnInteract?.Invoke();
            currentInteractTime = 0;
        }
      
        
    }

    private void LateUpdate()
    {
        if (_checkInteractTime <= currentInteractTime && currentInteractTime != 0)
        {
            currentInteractTime = Mathf.Clamp(_checkInteractTime, 0, interactTimeNeeded);
            OnInteractAbort?.Invoke();
        }
           

        _checkInteractTime -= Time.deltaTime;    
    }

    public void Focus() => onFocus?.Invoke();

    public void Unfocus() => onUnFocus?.Invoke();

    public bool CanInteract(float viewAngle,out float priority) 
    {
        priority = float.MinValue;
        Transform cam = Camera.main.transform;
        
        // Chequear si esta cerca
        if (!ViewHelper.IsNear(cam.position, _interactableCollider.transform.position, interactDistance)) return false;

        // Chequear si esta en vista
        if (ViewHelper.IsColliderInLineOfSight(cam.position, cam.forward, InteractablesManager.instance._sightBlock, _interactableCollider, out Vector3 hitPos))
        {
            // Chequear si esta en el campo de vision
            if (ViewHelper.IsInFOV(cam.position, cam.forward, hitPos, viewAngle, out priority)) return true;
        }

        return false;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
   
    void DrawRadius()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,interactDistance);
    }
}
