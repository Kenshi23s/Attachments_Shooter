using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Manager de cadencia de tiro
public class RateOfFireManager 
{

    [SerializeField] float _actualRateOfFire;
    [SerializeField] float _rateOfFireCD;
    bool _canShoot;
    public bool canShoot=> _canShoot;

    public float actualRateOfFire => _actualRateOfFire;

    public void Initialize()
    {
        _rateOfFireCD = 0;
        _canShoot = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnterCooldown()
    {

    }
}
