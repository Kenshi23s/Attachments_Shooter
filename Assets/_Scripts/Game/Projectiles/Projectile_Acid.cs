using FacundoColomboMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DebugableObject))]
public class Projectile_Acid : MonoBehaviour
{
    [SerializeField] Trap_DmgOvertime AcidLake;
    DebugableObject _debug;
    Rigidbody _rb;
    GameObject owner;

    LayerMask wallnfloor;

    int damage;
    float _acidRadius;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _debug = GetComponent<DebugableObject>(); _debug.AddGizmoAction(DrawVelocity);
      
    }

    void DrawVelocity()
    {
        DrawArrow.ForGizmo(transform.position,_rb.velocity,Color.green);
    }

    void Initialize(Tuple<GameObject, int,Vector3> x)
    {
        owner = x.Item1;
        damage = x.Item2;
        _rb.AddForce(x.Item3, ForceMode.VelocityChange);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner) return;

        if (collision.gameObject.TryGetComponent(out IDamagable x))
        {
            x.TakeDamage(damage);
            float angle = Vector3.Angle(transform.position.TryGetMeshCollision(Vector3.down,wallnfloor), Vector3.up);
            if (angle < 30f) { _debug.Log("Hago el lago de acido"); MakeLagoon(); } 
            Destroy(gameObject);
        }
    }
  
    void MakeLagoon()
    {
        Vector3 x = transform.position.TryGetMeshCollision(Vector3.down, wallnfloor);
        Trap_DmgOvertime y =Instantiate(AcidLake,x,Quaternion.Euler(Vector3.up));
        y.onStay += (x) => x.TakeDamage(2);      
    }

}
