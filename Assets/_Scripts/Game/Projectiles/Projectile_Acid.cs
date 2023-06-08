using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(Rigidbody))]
public class Projectile_Acid : MonoBehaviour
{
    [SerializeField] GameObject AcidLake;

    Rigidbody _rb;
    GameObject owner;

    int damage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    void Initialize(Tuple<GameObject, int,Vector3> x)
    {
        owner = x.Item1;
        damage = x.Item2;
        _rb.AddForce(x.Item3, ForceMode.VelocityChange);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner) return;
        
        if (other.gameObject.TryGetComponent(out IDamagable x ))
        {
            x.TakeDamage(damage);
        }
       
    }

}
