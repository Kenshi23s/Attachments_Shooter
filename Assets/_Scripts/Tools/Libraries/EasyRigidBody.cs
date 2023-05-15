using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EasyRigidBody 
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

   


   
}
