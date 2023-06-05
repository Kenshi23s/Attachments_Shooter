using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraPivots : MonoSingleton<PlayerCameraPivots>
{
   [SerializeField] Transform _viewFromInventory;
    public Transform ViewFromInventory => _viewFromInventory;

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
