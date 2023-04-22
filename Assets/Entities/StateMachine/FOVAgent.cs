using FacundoColomboMethods;
using UnityEngine;

public class FOVAgent : MonoBehaviour
{
   
  
    [SerializeField]float viewRadius;
    [SerializeField,Range(0,180)]float viewAngle;


    public bool inFOV(Vector3 obj)
    {
        Vector3 dir = obj - transform.position;

        if (dir.magnitude <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)
            {
                //aca deberia pasar la layer mask de paredes
                //chequear si el line of sight es el mismo que ia 1 o tiene alguna diferencia
                return ColomboMethods.InLineOffSight(transform.position, obj,LayerMask.GetMask());
            }
        }

        return false;
    }

    public void OnDrawGizmos()
    {
        


            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Gizmos.color = Color.white;

            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
            Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);

            Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);
        
     
    }

    
    //documentar esto, pq no entiendo la logica detras de la cuenta(lo vimos en IA 1)
    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
