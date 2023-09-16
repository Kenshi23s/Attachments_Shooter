using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractableComponent))]
public class GrabableObject : MonoBehaviour, IGrabable
{
    InteractableComponent _interactableComponent;

    public UnityEvent OnGrab, OnEquip,OnUnEquip ,OnRelease,OnInspect = new UnityEvent();

    public UnityEvent<IGadgetOwner> OnUse = new UnityEvent<IGadgetOwner>();

    public Transform Transform => transform;

    public IGadgetOwner Owner { get; private set; }

    private void Awake()
    {
        _interactableComponent = GetComponent<InteractableComponent>();
        _interactableComponent.OnInteract.AddListener(Grab);
    }

    public void Grab() => OnGrab?.Invoke();
  
    public void Equip() => OnEquip?.Invoke();
   
    public void Release() => OnRelease?.Invoke();
    
    public void Use(IGadgetOwner owner) => OnUse?.Invoke(owner);
    
    public void Inspect() => OnInspect?.Invoke();

    public void Unequip() => OnUnEquip?.Invoke();
  
    public void SetOwner(IGadgetOwner newOwner) => Owner = newOwner;
}

public interface IGrabable
{
    public bool HasOwner => Owner != null;
    public IGadgetOwner Owner { get; }
    public Transform Transform { get; }
    void SetOwner(IGadgetOwner newOwner);
    void Grab();  
    void Equip();
    void Release();
    void Use(IGadgetOwner owner);
    void Unequip();
    void Inspect();
}
