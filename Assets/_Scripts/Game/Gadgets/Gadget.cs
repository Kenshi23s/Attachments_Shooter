using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GrabableObject))]
public abstract class Gadget : MonoBehaviour
{
    public GrabableObject GrabableComponent { get; private set; }
    private void Awake()
    {
        GrabableComponent = GetComponent<GrabableObject>();
        GrabableComponent.OnUse.AddListener(() => UseGadget());
    }
    public abstract void AwakeContinue();
    public abstract bool UseGadget();
}
