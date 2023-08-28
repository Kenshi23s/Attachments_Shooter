using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractableComponent))]
public class GrabableObject : MonoBehaviour, IGrabable
{

    InteractableComponent _interactableComponent;

    public UnityEvent OnGrab, OnEquip,OnUnEquip ,OnRelease,OnUse,OnInspect = new UnityEvent();

    public Transform Transform => transform;

    public MonoBehaviour Owner { get; private set; }

 

    private void Awake()
    {
        _interactableComponent = GetComponent<InteractableComponent>();
        _interactableComponent.OnInteract.AddListener(Grab);
    }

    public void Grab() => OnGrab?.Invoke();
  
    public void Equip() => OnEquip?.Invoke();
   
    public void Release() => OnRelease?.Invoke();
    
    public void Use() => OnUse?.Invoke();
    
    public void Inspect() => OnInspect?.Invoke();

    public void Unequip() => OnUnEquip?.Invoke();
  
    public void SetOwner(MonoBehaviour newOwner)
    {
        Owner = newOwner;
    }
}
public interface IGrabable
{
    public bool HasOwner => Owner != null;
    public MonoBehaviour Owner { get; }
    public Transform Transform { get; }
    void Grab();  
    void Equip();
    void Release();
    void Use();
    void Unequip();
    void Inspect();
}
