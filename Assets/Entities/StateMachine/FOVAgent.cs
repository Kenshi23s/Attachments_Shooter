using FacundoColomboMethods;
using System;
using UnityEngine;

[RequireComponent(typeof(DebugableObject))]
public class FOVAgent : MonoBehaviour
{
   
  
    [SerializeField]float _viewRadius;
    public float viewRadius => _viewRadius;
    float _sqrViewRadius;
    [SerializeField,Range(0,360)]float viewAngle;

    private void Awake()
    {
        GetComponent<DebugableObject>().AddGizmoAction(FovGizmos);
    }
    public bool IN_FOV(Vector3 obj)
    {
        Vector3 dir = obj - transform.position;

        if (dir.magnitude <= _viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)                     
                return ColomboMethods.InLineOffSight(transform.position, obj,LayerMask.GetMask());      
        }
        return false;
    }

    public bool IN_FOV(Vector3 obj, float viewRadius)
    {
        Vector3 dir = obj - transform.position;

        if (dir.magnitude <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)
                return ColomboMethods.InLineOffSight(transform.position, obj, LayerMask.GetMask());
        }

        return false;
    }

    public void FovGizmos()
    {        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
        
        Gizmos.color = Color.white;
        
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
        
        Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);
        
        Gizmos.DrawLine(transform.position, transform.position + lineA * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * _viewRadius);    
    }

    
    //documentar esto, pq no entiendo la logica detras de la cuenta(lo vimos en IA 1)
    Vector3 GetVectorFromAngle(float angle) => new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
   

}
