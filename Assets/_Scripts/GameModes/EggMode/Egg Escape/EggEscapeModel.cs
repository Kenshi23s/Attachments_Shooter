using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(GrabableObject))]
public class EggEscapeModel : MonoBehaviour
{

    public InteractableComponent InteractComponent { get; private set; }
    public GrabableObject GrabableComponent { get; private set; }
    public Egg_VFXHandler VFX { get; private set; }
    public GameObject view;
    Rigidbody rb;
 

    public UnityEvent OnGrab, OnRelease;

    private void Awake()
    {
        GrabableComponent = GetComponent<GrabableObject>();

        GrabableComponent.OnGrab.AddListener(DisableEggLogic);
        GrabableComponent.OnRelease.AddListener(EnableEggLogic);

        rb = GetComponent<Rigidbody>();
        enabled = false;
    }

    public void DisableEggLogic()
    {
        Debug.Log("Deshabilito Logica Huevo");
        enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        GetComponent<Collider>().enabled = false;

    }

    public void EnableEggLogic()
    {
        Debug.Log("Habilito Logica Huevo");
        rb.isKinematic = false;
        rb.useGravity = true;
        enabled = true;
        GetComponent<Collider>().enabled = true;
    }



}

