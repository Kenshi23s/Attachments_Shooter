using System.Collections;
using System;
using UnityEngine;
//Manager de cadencia de tiro
[DisallowMultipleComponent]
public class RateOfFireHandler: MonoBehaviour
{
    //deberia tener un rate of fire manager para cada tipo de cadencia, uno para automatico/singleShot y otro para rafaga
    //respondiendo a lo de arriba, quizas no
    Gun _gun;
    [SerializeField,Range(0.1f,3f)] float _actualRateOfFire;
    public float actualRateOfFire => _actualRateOfFire;

    [SerializeField,Tooltip("Variable para ver el cd restante, no tocar")]
    float _rateOfFireCD;

    public bool canShoot=> _canShoot;
    bool _canShoot;

   
 
    //public void Initialize(Gun gun)
    //{
    //    _rateOfFireCD = 0;
    //    _canShoot = true;
    //    this._gun = gun;
    //    gun.onShoot += StartCooldown;

    //}

    private void Awake()
    {
        _gun = GetComponent<Gun>();
        _rateOfFireCD = 0;
        _canShoot = true;
        _gun.onShoot += StartCooldown;
    }


    void StartCooldown() 
    {
        if (canShoot)
        {
            _canShoot = false;
            _rateOfFireCD = _actualRateOfFire;
            _gun.everyTick += CoolDownRF;
        }
    }
   
    void CoolDownRF()
    {
        _rateOfFireCD -= Time.deltaTime;
        if (_rateOfFireCD <= 0)
        {

            _rateOfFireCD = _actualRateOfFire;
            _canShoot = true;
            _gun.everyTick -= CoolDownRF;
        }
    }
}
