using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GrabableObject))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Gadget : MonoBehaviour
{
    public GrabableObject GrabableComponent { get; private set; }
    public Rigidbody GadgetRB { get; private set; }
    Collider _collider;
    private void Awake()
    {
        GrabableComponent = GetComponent<GrabableObject>();
        GadgetRB = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();   

        GrabableComponent.OnUse.AddListener(x => UseGadget(x));
        GrabableComponent.OnGrab.AddListener(() => ActivatePhysics(false));
        GrabableComponent.OnRelease.AddListener(() => ActivatePhysics(true));
        AwakeContinue();
    }

    protected virtual void ActivatePhysics(bool arg)
    {
        Debug.Log(arg ? "Activo" : "Desactivo" +" Fisicas y Collider");
        GadgetRB.useGravity = arg;
        GadgetRB.isKinematic = !arg;
        _collider.enabled = arg;

    }

    public abstract void AwakeContinue();
    public abstract bool UseGadget(IGrabableOwner owner);
}
