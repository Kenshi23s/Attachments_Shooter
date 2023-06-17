using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(DebugableObject))]
public class InteractableComponent : MonoBehaviour, IInteractable
{
   



    //UIManager _ui;
    [SerializeField]float interactDistance;
    [SerializeField] Collider _interactableCollider;


    public UnityEvent onFocus,onUnFocus,OnInteract;



    private void Start()
    {
        InteractablesManager.instance.AddInteractableObject(this);

        //_interactableCollider=GetComponent<Collider>();
        var debug = GetComponent<DebugableObject>();     
        OnInteract.AddListener(() => debug.Log("Interactuan conmigo"));
        onFocus.AddListener(() => debug.Log("Me focusean"));
        onUnFocus.AddListener(() => debug.Log("Me dejaron de focusear conmigo"));
    }

    private void OnDestroy()
    {
        InteractablesManager.instance.RemoveInteractableObject(this);
        onFocus.RemoveAllListeners();
        onUnFocus.RemoveAllListeners();
        OnInteract.RemoveAllListeners();
    }

    // En vez de estar constantemente agregando y removiendo el interactuable, tal vez sea mejor que la interfaz tenga un metodo 'isActive'
    // y que mediante esta se determine si se debe interactuar o no con ella.

    public void Interact()
    {
        
        OnInteract?.Invoke();
        
    }

    public void Focus()
    {
        onFocus?.Invoke();
    }


    public void Unfocus()
    {
        onUnFocus?.Invoke();
    }

    public bool CanInteract(float viewAngle,out float priority) 
    {
        priority = float.MinValue;
        Transform cam = Camera.main.transform;
        Debug.Log("Triying To Interact...");
        // Chequear si esta cerca
        if (!ViewHelper.IsNear(cam.position, _interactableCollider.transform.position, interactDistance)) return false;

        // Chequear si esta en el campo de vision
        if (!ViewHelper.IsInFOV(cam.position, cam.forward, _interactableCollider.transform.position, viewAngle, out priority)) return false;

        // Chequear si esta en vista
        return ViewHelper.IsColliderInLineOfSight(cam.position, _interactableCollider.transform.position, InteractablesManager.instance._sightBlock, _interactableCollider);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
   
}
