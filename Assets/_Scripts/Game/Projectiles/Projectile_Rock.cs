using FacundoColomboMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(PausableObject))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile_Rock : MonoBehaviour,IDamagable
{
    [SerializeField]
    int life = 100;
   public event Action<IEnumerable<Collider>> onExplosion;
    [SerializeField]float _radius;

    [SerializeField] Rigidbody _rb;
    GameObject _owner;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        GetComponent<PausableObject>().onPause += () => StartCoroutine(Pause());
        Destroy(gameObject, 20f);
    }
    IEnumerator Pause()
    {
        var x = _rb.velocity;
        _rb.velocity = Vector3.zero;
        var y = _rb.useGravity;
        _rb.useGravity = false;
        yield return new WaitUntil(ScreenManager.IsPaused);
        _rb.velocity = x;
        _rb.useGravity = y;
    }
    public void Iniitialize(GameObject owner, float _explosionRadius)
    {
        _owner = owner;
        gameObject.name= $"[{owner.name}] {gameObject.name}";
        _radius = _explosionRadius;
    }

    public void LaunchProjectile(Vector3 force)
    {
        _rb.useGravity = true;
        _rb.AddForce(force,ForceMode.Impulse);
        transform.forward = force.normalized;
    }
    public void AddDamageOverTime(int TotalDamageToDeal, float TimeAmount) => TakeDamage(TotalDamageToDeal);

   


    public DamageData TakeDamage(int dmgToDeal)
    {
        life -= dmgToDeal;
        DamageData x = new DamageData();
        x.damageDealt = dmgToDeal;
        x.victim = this;
        x.wasCrit = false;
        
        if (life<=0) StartCoroutine(DestroyCoroutine());

        return x;
    }

    IEnumerator DestroyCoroutine()
    {
        this.GetComponent<BoxCollider>().enabled=false;
        for (int i = 0; i < 2; i++) yield return null;

        Destroy(gameObject);

    }
    public DamageData TakeDamage(int dmgToDeal, Vector3 hitPoint)
    {
        return TakeDamage(dmgToDeal);
    }

    


    private void OnTriggerEnter(Collider other)
    {
        onExplosion?.Invoke(transform.position.GetItemsOFTypeAround<Collider>(_radius).Where(x => x != this));
        onExplosion = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void AddKnockBack(Vector3 force)
    {
       
    }

    public Vector3 Position()
    {
        return transform.position;
    }
}
