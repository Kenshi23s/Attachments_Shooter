using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static ProceduralPlatform;
[RequireComponent(typeof(Rigidbody))]
public class SolidCreator_Projectile : MonoBehaviour
{

    public Rigidbody RB { get; private set; }
    public List<GameObject> Owners { get; private set; }
    PlatformsParameters parameters;
    Vector3 InitialPosition;

    Func<ProceduralPlatform> GetSample = () => ProceduralPlatformManager.instance.pool.Get();

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        GetSample = () => ProceduralPlatformManager.instance.pool.Get();
    }

    public void LaunchProjectile(Vector3 ImpulseForce, IEnumerable<GameObject> newOwner,PlatformsParameters newParameters)
    {
        InitialPosition = transform.position;
        Owners = newOwner.ToList();
        RB.AddForce(ImpulseForce,ForceMode.Impulse);
        parameters = newParameters;
    }

    private void LateUpdate()
    {
        if (CollisionInbound(out var normal,out var point))
        {
          
            //relleno datos restantes
            parameters.CrossResult = GetPerpendicular(transform.forward, normal);
            parameters.CenterPosition = point;
            //creo la plataforma
            Debug.Log("Collider");
            var x = GetSample();
            x.CreatePlatform(point, parameters);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Owners.Where(x => x != other.gameObject).Any()) return;

        Vector3 closestPoint = other.ClosestPointOnBounds(transform.position);
        Ray ray = new Ray(transform.position , RB.velocity.normalized);
        if (other.Raycast(ray, out var hit , float.MaxValue))
        {
            

        }

       
      
        

    }

    bool CollisionInbound(out Vector3 normal,out Vector3 point)
    {
        normal = Vector3.zero;
        point = Vector3.zero;

        if (Physics.Raycast(transform.position,RB.velocity, out var hit,(1 + RB.velocity.magnitude) * Time.fixedDeltaTime, parameters.SolidMasks, QueryTriggerInteraction.Ignore))
        {
            normal = hit.normal;
            point = hit.point;
        }
        return normal != Vector3.zero && point != Vector3.zero ;
    }

   
}
