using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraPivots : MonoSingleton<PlayerCameraPivots>
{
   
    [field: SerializeField] public Transform ViewFromInventory { get; private set; }
   

    protected override void SingletonAwake()
    {   
        enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        DrawArrow.ForGizmo(transform.position,transform.forward * 10,Color.blue,1);
    }


}
