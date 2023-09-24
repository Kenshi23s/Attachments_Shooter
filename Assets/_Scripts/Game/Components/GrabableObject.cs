using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractableComponent))]
public class GrabableObject : MonoBehaviour, IGrabable
{
    InteractableComponent _interactableComponent;

    public UnityEvent OnGrab, OnEquip,OnUnEquip ,OnRelease,OnInspect = new UnityEvent();

    public UnityEvent<IGrabableOwner> OnUse = new UnityEvent<IGrabableOwner>();

    public Transform Transform => transform;

    public IGrabableOwner Owner { get; private set; }

    //quiero q esto lo rellene el dueño del componente
    public bool IsPriorityObject => throw new System.NotImplementedException();

    private void Awake()
    {
        _interactableComponent = GetComponent<InteractableComponent>();
        _interactableComponent.OnInteract.AddListener(Grab);
        //_interactableComponent.SetFocusCondition(() => );
    }

    public void Grab() => OnGrab?.Invoke();
  
    public void Equip() => OnEquip?.Invoke();
   
    public void Release() => OnRelease?.Invoke();
    
    public void Use(IGrabableOwner owner) => OnUse?.Invoke(owner);
    
    public void Inspect() => OnInspect?.Invoke();

    public void Unequip() => OnUnEquip?.Invoke();
  
    public void SetOwner(IGrabableOwner newOwner) => Owner = newOwner;
}

public interface IGrabable
{
    public bool HasOwner => Owner != null;
    public bool IsPriorityObject { get; }
    public IGrabableOwner Owner { get; }
    public Transform Transform { get; }
    void SetOwner(IGrabableOwner newOwner);
    void Grab();  
    void Equip();
    void Release();
    void Use(IGrabableOwner owner);
    void Unequip();
    void Inspect();
}
