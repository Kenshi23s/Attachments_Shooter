using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletStats
{
    public enum Stat
    {
        Speed,
        Damage,


    }
    Dictionary<Stat, float> stats;
    Action onHit;
    Action onCritHit;
    Action onKill;
    Action onCritKill;
}
[RequireComponent(typeof(Rigidbody))]
public abstract class BaseBulltet : MonoBehaviour
{
    BulletStats mystats;
    Rigidbody rb;

  

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Initialize(BulletStats mystats)
    {
        this.mystats = mystats;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
