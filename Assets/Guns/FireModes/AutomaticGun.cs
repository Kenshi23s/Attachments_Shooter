using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RateOfFireHandler))]
public abstract class AutomaticGun : Gun
{
    protected RateOfFireHandler _rateFireHandler;

    protected override void OptionalInitialize()
    {
        //_rateFireHandler.Initialize(this);
        base.OptionalInitialize();
        _rateFireHandler = GetComponent<RateOfFireHandler>();

    }


}