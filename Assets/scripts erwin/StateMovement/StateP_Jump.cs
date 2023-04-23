using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateP_Jump : StatePlayerMovement
{
    public override void Initialize()
    {

    }

    public override void OnVirtualUpdate()
    {
        Lurch();
        base.OnVirtualUpdate();
    }
}
