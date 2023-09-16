using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(DebugableObject))]
[DisallowMultipleComponent]
public class InteractableComponent : MonoBehaviour, IInteractable
{

    [SerializeField] float interactDistance = 10;
    public float interactTimeNeeded , currentInteractTime;
    float _checkInteractTime;
    [SerializeField] Collider _interactableCollider;
    Outline outlineInteractable;
    public UnityEvent onFocus = new UnityEvent(), onUnFocus = new UnityEvent(), OnInteract = new UnityEvent(),
        onTryingToInteract = new UnityEvent(), OnStartInteracting = new UnityEvent(),
        OnInteractAbort = new UnityEvent(), OnInteractFail = new UnityEvent();


    public List<Func<bool>> InteractConditions = new List<Func<bool>>();
    Transform cam;
    DebugableObject _debug;


    public float NormalizedProgress => currentInteractTime / interactTimeNeeded;

    public Func<bool> CanFocus { get; private set; } = () => true;

    public void SetFocusCondition(Func<bool> newCondition)
    {
        CanFocus = newCondition;
    }

    private void Awake()
    {
        cam = Camera.main.transform;
        if (_interactableCollider != null)
        {
            outlineInteractable = _interactableCollider.GetComponent<Outline>();
            if (outlineInteractable == null)
                outlineInteractable = _interactableCollider.gameObject.AddComponent<Outline>();
        }
        else
        {
            outlineInteractable = GetComponent<Outline>();
            if (outlineInteractable == null)
                outlineInteractable = gameObject.AddComponent<Outline>();
        }


    }

    private void Start()
    {
        InteractablesManager.instance.AddInteractableObject(this);

        outlineInteractable.enabled = false;
        outlineInteractable.OutlineMode = Outline.Mode.OutlineVisible;

        //_interactableCollider=GetComponent<Collider>();
        _debug = GetComponent<DebugableObject>();
        //añado un debug a todos los listener
        #region listenerDebugs
        OnInteract.AddListener(() => _debug.Log("Interactuan conmigo!"));
        onFocus.AddListener(() => _debug.Log("Me focusean"));
        onUnFocus.AddListener(() => _debug.Log("Me dejaron de focusear"));
        OnInteractFail.AddListener(() => _debug.Log("No se puede interactuar, alguna condicion no se cumplio"));
        OnInteractAbort.AddListener(() => _debug.Log("Se dejo de presionar el input de interactuar"));
        OnStartInteracting.AddListener(() => _debug.Log("Se empezo el progreso para interactuar!"));
        #endregion

        onFocus.AddListener(() => outlineInteractable.enabled = true);

        onUnFocus.AddListener(() => outlineInteractable.enabled = false);

        _debug.AddGizmoAction(DrawRadius);
    }

    private void OnDestroy() => NoMoreInteraction();


    public void NoMoreInteraction()
    {
        InteractablesManager.instance.RemoveInteractableObject(this);

        onFocus.RemoveAllListeners();
        onUnFocus.RemoveAllListeners();
        OnInteract.RemoveAllListeners();
        onTryingToInteract.RemoveAllListeners();
        OnInteractAbort.RemoveAllListeners();
        CanFocus = () => false;
    }

    // En vez de estar constantemente agregando y removiendo el interactuable, tal vez sea mejor que la interfaz tenga un metodo 'isActive'
    // y que mediante esta se determine si se debe interactuar o no con ella.


    public bool Interact()
    {
        if (InteractConditions.Any())
        {
            foreach (var item in InteractConditions)
             if (!item.Invoke())
             {
                 _debug.Log("Las condiciones dieron falso, no se puede interactuar");
                 OnInteractFail?.Invoke();
                 return false;
             }

        }

        _checkInteractTime = currentInteractTime += Time.deltaTime;

        onTryingToInteract?.Invoke(); onTryingToInteract.RemoveListener(StartInteracting);
        if (currentInteractTime >= interactTimeNeeded)
        {
            OnInteract?.Invoke();
            currentInteractTime = 0;
            return true;
        }
        return false;
    }




    private void LateUpdate()
    {
        if (_checkInteractTime <= currentInteractTime && currentInteractTime != 0)
        {
            currentInteractTime = Mathf.Clamp(_checkInteractTime, 0, interactTimeNeeded);
            OnInteractAbort?.Invoke();
            onTryingToInteract.AddListener(StartInteracting);
        }

        _checkInteractTime -= Time.deltaTime;
    }
    void StartInteracting() => OnStartInteracting?.Invoke();

    public void Focus() => onFocus?.Invoke();

    public void Unfocus() => onUnFocus?.Invoke();

    public bool CanInteract(float viewAngle, out float priority)
    {
        priority = float.MinValue;

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
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
