using FacundoColomboMethods;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DebugableObject))]
public class Projectile_Acid : MonoBehaviour
{
    [SerializeField] Trap_DmgOvertime AcidLake;
    DebugableObject _debug;
    Rigidbody _rb;
    GameObject owner;

    [SerializeField] LayerMask wallnfloor;

    int damage;
    float _acidRadius;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _debug = GetComponent<DebugableObject>(); _debug.AddGizmoAction(DrawVelocity);
      
    }

    void DrawVelocity() => DrawArrow.ForGizmo(transform.position, _rb.velocity, Color.green);
 

    public void Initialize(Tuple<GameObject, int,Vector3> x)
    {
        owner = x.Item1;
        damage = x.Item2;
        _rb.AddForce(x.Item3, ForceMode.VelocityChange);

    }

    private void OnCollisionEnter(Collision collision)
    {
        _debug.Log("Colision con "+ collision.gameObject);
        if (collision.gameObject == owner) return;

        if (collision.gameObject.TryGetComponent(out IDamagable x))
        {
            x.TakeDamage(damage);
            
        }
        Tuple<float,Vector3> b = transform.position.GetNormalAngleOfFloor(wallnfloor);
        if (b.Item1 < 70f) { _debug.Log("Hago el lago de acido"); MakeLagoon(b.Item2); } 
        else 
        {
            _debug.Log("No hago el lago, el angulo de la normal es "+ b.Item1); ;
        }
        
        Destroy(gameObject);
    }
  
    void MakeLagoon(Vector3 forward)
    {
        Vector3 x = transform.position.TryGetMeshCollision(Vector3.down, wallnfloor);
        Trap_DmgOvertime y = Instantiate(AcidLake,x,Quaternion.Euler(forward));
        y.onStay += (x) =>
        {
            x.TakeDamage(2); Debug.Log("el acido hace daño");
        };
        Destroy(y, 6f);
    }
}
