using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class sc_RProbe : MonoBehaviour
{
    public ReflectionProbe myReflection;
    public float drownedSpace;
    public float skySpace;
    public bool showGizmos;
    public LayerMask myCollider;
    public float nearDistance;

    public void ScaleProbe(float scale, float ExtraBlend)
    {
        scale += ExtraBlend;
        myReflection.blendDistance = ExtraBlend;
        myReflection.size = new Vector3(scale,scale,scale);
    }

    public bool CheckDrowned()
    {
        if (Physics.OverlapSphere(transform.position, drownedSpace, myCollider).Any())
        {
            return true;
        }

        return false;
    }

    public bool CheckSkySpace()
    {
        if (Physics.Raycast(transform.position, Vector3.up, skySpace, myCollider))
        {
            return true;
        }

        return false;
    }

    public bool CheckNearObjects()
    {
        if (Physics.OverlapSphere(transform.position, nearDistance, myCollider).Any())
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos) 
        {
            Gizmos.color = Color.white;

            Gizmos.DrawSphere(transform.position, drownedSpace);
        }
        
    }
}
