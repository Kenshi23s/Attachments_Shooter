using FacundoColomboMethods;
using UnityEngine;

public class FOVAgent
{
    public FOVAgent(Transform _myTransform)
    {
        this._myTransform = _myTransform;
    }
    Transform _myTransform;
    public float viewRadius;
    public float viewAngle;


    public bool inFOV(Vector3 obj)
    {
        Vector3 dir = obj - _myTransform.position;

        if (dir.magnitude <= viewRadius)
        {
            if (Vector3.Angle(_myTransform.forward, dir) <= viewAngle / 2)
            {
                //aca deberia pasar la layer mask de paredes
                //chequear si el line of sight es el mismo que ia 1 o tiene alguna diferencia
                return ColomboMethods.InLineOffSight(_myTransform.position, obj,LayerMask.GetMask());
            }
        }

        return false;
    }

    public void FovDrawGizmos()
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(_myTransform.position, viewRadius);

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(_myTransform.position, viewRadius);

        Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + _myTransform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + _myTransform.eulerAngles.y);

        Gizmos.DrawLine(_myTransform.position, _myTransform.position + lineA * viewRadius);
        Gizmos.DrawLine(_myTransform.position, _myTransform.position + lineB * viewRadius);
        Gizmos.DrawLine(_myTransform.position, _myTransform.position + lineB * viewRadius);
    }
    //documentar esto, pq no entiendo la logica detras de la cuenta(lo vimos en IA 1)
    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
