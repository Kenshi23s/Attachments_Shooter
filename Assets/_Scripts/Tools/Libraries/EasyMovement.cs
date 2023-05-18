using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct FlockingParameters
{
    public IEnumerable<IA_Movement> targets;
    public Transform myTransform;
    public float maxForce;
    public float viewRadius;

}
public static class EasyMovement 
{

   public static Rigidbody MoveTowards(this Rigidbody rb,Vector3 dir, float force)
   {
      rb.velocity = rb.velocity + dir.normalized * force * Time.deltaTime;
      return rb;
   }

   public static Rigidbody ClampVelocity(this Rigidbody rb, float _maxSpeed)
   {
       rb.velocity= Vector3.ClampMagnitude(rb.velocity, _maxSpeed);
       return rb;
   }

    public static Vector3 CalculateSteering(this Vector3 velocity, Vector3 desired, float steeringForce) => (desired - velocity) * steeringForce;

    #region Flocking
    public static Vector3 GroupAlignment(this IEnumerable<IA_Movement> targets, Transform myTransform, float viewRadius,float maxForce)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (IA_Movement item in targets)
        {
            Vector3 dist = item.transform.position - myTransform.position;

            if (dist.magnitude <= viewRadius)
            {
                desired += item.velocity;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;

        desired.Normalize();
        desired *= maxForce;

        return desired;
    }

    public static Vector3 Cohesion(this IEnumerable<IA_Movement> targets,Transform myTransform,float maxForce,float viewRadius)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in targets.Where(x => x.transform!=myTransform))
        {
            Vector3 dist = item.transform.position - myTransform.position;

            if (dist.magnitude <= viewRadius)
            {
                desired += item.transform.position;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;
        desired -= myTransform.position;

        desired.Normalize();
        desired *= maxForce;

        return desired;
    }

    public static Vector3 Separation(this IEnumerable<IA_Movement> targets, Transform myTransform, float maxForce, float viewRadius)
    {
        Vector3 desired = Vector3.zero;
        foreach (var item in targets.Where(x=> x.transform !=myTransform))
        {
            Vector3 dist = item.transform.position - myTransform.position;

            if (dist.magnitude <= viewRadius)
                desired += dist;
        }

        if (desired == Vector3.zero)
            return desired;

        desired = -desired;

        desired.Normalize();
        desired *= maxForce;

        return desired;
    }
    #endregion



}
