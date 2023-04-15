using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Turret : Enemy
{
    [Header("Turret")]
    [SerializeField] float _detectionRadius;
    [SerializeField] int   _bulletDamage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _rotationSpeed;

    [SerializeField] GameObject bulletPrefab;

   

    public override int OnTakeDamage(int dmgDealt)
    {
       return lifeHandler.Damage(dmgDealt);
    }

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //bool AlignCanon(Vector3 playerPos)
    //{
    //    Vector3 dir = playerPos - transform.position;
    //}
    public override void Pause()
    {
        return;
    }

    public override void Resume()
    {
        return;
    }

    public override bool WasCrit() => false;

}
