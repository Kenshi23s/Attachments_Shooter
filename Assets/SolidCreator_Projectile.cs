using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ProceduralPlatform;
[RequireComponent(typeof(Rigidbody))]
public class SolidCreator_Projectile : MonoBehaviour
{

    public Rigidbody RB { get; private set; }
    public List<GameObject> Owners { get; private set; }
    public ProceduralPlatform Sample;
    PlatformsParameters parameters;
    Vector3 InitialPosition;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    public void LaunchProjectile(Vector3 ImpulseForce, IEnumerable<GameObject> newOwner,PlatformsParameters newParameters)
    {
        InitialPosition = transform.position;
        Owners = newOwner.ToList();
        RB.AddForce(ImpulseForce,ForceMode.Impulse);
        parameters = newParameters;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!Owners.Where(x => x != other.gameObject).Any()) return;

        Vector3 closestPoint = other.ClosestPointOnBounds(transform.position);
        Ray ray = new Ray(transform.position, (closestPoint - transform.position).normalized);
        if (other.Raycast(ray, out var hit , float.MaxValue))
        {
            Vector3 dir = transform.position - InitialPosition;
            //relleno datos restantes
            parameters.CrossResult = GetPerpendicular(dir,hit.normal);
            parameters.CenterPosition = hit.point;
            //creo la plataforma
            Debug.Log("Collider");
            var x = Instantiate(Sample,hit.point,Quaternion.identity);
            x.CreatePlatform(hit.point,parameters);

        }

        Destroy(gameObject,0.2f);
      
        

    }
}
