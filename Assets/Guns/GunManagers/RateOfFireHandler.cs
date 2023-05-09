using System.Collections;
using System;
using UnityEngine;
//Manager de cadencia de tiro
[DisallowMultipleComponent]
public class RateOfFireHandler: MonoBehaviour
{
    //deberia tener un rate of fire manager para cada tipo de cadencia, uno para automatico/singleShot y otro para rafaga
    //respondiendo a lo de arriba, quizas no
    [SerializeField,Range(0.1f,3f)] float _actualRateOfFire;
    public float actualRateOfFire => _actualRateOfFire;

    [SerializeField,Tooltip("Variable para ver el cd restante, no tocar")]
    float _rateOfFireCD;

    public bool canShoot=> _canShoot;
    bool _canShoot;

   
    GunFather gun;
    public void Initialize(GunFather gun)
    {
        _rateOfFireCD = 0;
        _canShoot = true;
        this.gun = gun;
        gun.onShoot += StartCooldown;

    }

    //podria encapsular el primero en un lambda, lo dejo asi para mejor lectura

    void StartCooldown() 
    {
        if (canShoot)
        {
            _canShoot = false;
            _rateOfFireCD = _actualRateOfFire;
            gun.everyTick += CoolDownRF;
        }
    }
    //el segundo no lo podria encapsular pq se perderia la referencia del metodo al substraerlo del evento
    void CoolDownRF()
    {
        _rateOfFireCD -= Time.deltaTime;
        if (_rateOfFireCD <= 0)
        {

            _rateOfFireCD = _actualRateOfFire;
            _canShoot = true;
            gun.everyTick -= CoolDownRF;
        }
    }
}
