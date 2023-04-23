using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateP_Jump : StatePlayerMovement
{
    public void Initialize()
    {

    }

    public override void OnVirtualUpdate()
    {
        Lurch();
        base.OnVirtualUpdate();
    }
}
