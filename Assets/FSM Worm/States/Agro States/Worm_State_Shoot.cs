using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm_State_Shoot : Worm_State
{
    public Worm_State_Shoot(Enemy_Worm worm,float _requiredCharge) : base(worm)
    {
        this._requiredCharge = _requiredCharge;
    }

    float actualCharge, _requiredCharge;

    public override void OnEnter()
    {        
        actualCharge = 0;
    }

    public override void OnUpdate()
    {
        actualCharge += Time.deltaTime;
        
    }
}
