using FacundoColomboMethods;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(PausableObject))]
public class Projectile_Acid : MonoBehaviour
{
    [SerializeField] Trap_DmgOvertime AcidLake;
    DebugableObject _debug;
    Rigidbody _rb;
    GameObject owner;
  

    [SerializeField] LayerMask wallnfloor;

    int damage;
    


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _debug = GetComponent<DebugableObject>(); _debug.AddGizmoAction(DrawVelocity);
        GetComponent<PausableObject>().onPause += () => StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        var x = _rb.velocity;
        _rb.velocity = Vector3.zero;
        var z = _rb.useGravity;
        _rb.useGravity = false;
     
        yield return new WaitUntil(ScreenManager.IsPaused);
        _rb.useGravity = z;
        _rb.velocity = x;
    }

    void DrawVelocity() => DrawArrow.ForGizmo(transform.position, _rb.velocity, Color.green);
 

    public void Initialize(GameObject _owner,int _damage,Vector3 force)
    {
        owner = _owner;
        damage = _damage;
        _rb.AddForce(force, ForceMode.VelocityChange);
    }


    private void OnCollisionEnter(Collision collision)
    {
        _debug.Log("Colision con " + collision.gameObject);
        if (collision.gameObject == owner) return;

        if (collision.gameObject.TryGetComponent(out IDamagable x))
        {
            x.TakeDamage(damage);

        }
        Tuple<float, Vector3> b = transform.position.GetNormalAngleOfFloor(wallnfloor);
        if (b.Item1 < 70f) { _debug.Log("Hago el lago de acido"); MakeLagoon(b.Item2); }
        else
        {
            _debug.Log("No hago el lago, el angulo de la normal es " + b.Item1); ;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        
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
