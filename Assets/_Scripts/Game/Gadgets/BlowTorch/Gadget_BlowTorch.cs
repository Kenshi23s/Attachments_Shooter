using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gadget_BlowTorch : Gadget
{

    BurnCollider BurnCollider;
    [SerializeField] GameObject FireParticle;

    Action switchMode = delegate { };

    public override void AwakeContinue()
    {
        BurnCollider = GetComponentInChildren<BurnCollider>();
    }

    private void Start()
    {
        BurnCollider.OnActivate.AddListener(() => FireParticle.SetActive(true));
        BurnCollider.OnDeactivate.AddListener(() => FireParticle.SetActive(false));
        FireParticle.SetActive(false);
        TurnTorchOff();
    }


    void TurnTorchOn()
    {
        BurnCollider.ActivateCollider();
        switchMode = TurnTorchOff;
    }

    void TurnTorchOff()
    {
        BurnCollider.DeactivateCollider();
        switchMode = TurnTorchOn;
    }


    public override bool UseGadget(IGrabableOwner owner)
    {
        switchMode.Invoke();
        return true;
    }
}
