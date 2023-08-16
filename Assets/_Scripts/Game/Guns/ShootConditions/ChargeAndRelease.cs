using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAndRelease : ChargeCondition
{
    public event Action OnRelease;
    public override bool WasChargeTimeMet()
    {
        //despues el mouse 0 lo cambiariamos con otra cosa cuando tengamos el MVC ya puesto
        if (CurrentCharge >= MaxCharge && Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnRelease?.Invoke();
            return true;
        }
        return false;
       
    }
}
