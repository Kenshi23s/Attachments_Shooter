using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class BurnCollider : MonoBehaviour
{
    public List<IBurnable> Owners = new();

    public float StackScalar;

    Collider myCollider;

    public UnityEvent OnHit = new UnityEvent();
    public UnityEvent OnActivate = new UnityEvent();
    public UnityEvent OnDeactivate = new UnityEvent();

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
        
    }

    public void ActivateCollider()
    {
        myCollider.enabled = true;
    }

    public void DeactivateCollider()
    {
        myCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"toque {other.transform.name}");
        if (!other.transform.TryGetComponent(out IBurnable x)) return;

        if (Owners.Contains(x)) return;

        if (x == null) return;

        //creo que esto no me da exactamente el punto de colision
        Debug.Log("Lo quemo");
        Vector3 impactOn = other.ClosestPoint(transform.position);
        float y = StackScalar * Time.deltaTime;
        Debug.Log(y);
        x.AddStacks(y, impactOn);


    }



}
