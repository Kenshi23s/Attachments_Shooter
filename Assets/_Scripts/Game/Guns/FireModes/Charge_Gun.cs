using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class Charge_Gun : Gun
{
    public bool isCharging { get; private set; }
   
    //si se quiere testear el arma, se debe hacer hijo de gunmanager(esta dentro del player)
    //y eliminar la otra arma(a menos que ya haya hecho codigo para swapear armas)

    [SerializeField,Tooltip("Tiempo de carga necesario"),Header("Charge Stats")]float _requiredTime;
     float _checkTime;
    //tiempo de carga actual
    float _currentTime;
    public float currentTime { get => _currentTime; private set => _currentTime = Mathf.Clamp(value,0,_requiredTime); }
    //cooldown actual de disparo
    float _currentCooldown;
    [SerializeField, Tooltip("cooldown antes de empezar a cargar otro tiro")] float _cooldownTime;

   
    //cuando se oprime el gatillo
    public override void Trigger()
    {
        //me guardo el tiempo actual
        _checkTime = currentTime;
        //si no puedo disparar aun, is charging es falso
        if (!ShootCondition()) 
        {
             _debug.Log($"Arma en cooldown");
             isCharging = false; return; 
        }

        isCharging = true;  currentTime += Time.deltaTime;
        _debug.Log($"Se esta cargando, la carga sube a {currentTime}");
        if (currentTime >= _requiredTime)
        {
            _debug.Log($"Se termino de cargar, entro en cooldown");
            Shoot(); CallOnShootEvent(); StartCoroutine(CooldownTime());     
        }
    }

    //mas adelante estaria bueno no tenerlo en corrutina, para saber cuanto bajo la carga, pero por ahora...
    IEnumerator CooldownTime()
    {
        _currentCooldown = _cooldownTime;
        currentTime = 0;
        isCharging = false;
        yield return new WaitForSeconds(_cooldownTime);
        _currentCooldown = 0;
    }
    private void LateUpdate()
    {
        //si el tiempo de chequeo es mayor a el tiempo q me habia guardado
        if (_checkTime >= currentTime)
        {
            //resto carga
            currentTime -= Time.deltaTime;
            _checkTime = currentTime;
            _debug.Log($"Se dejo de cargar, la carga disminuye a {currentTime}");
            if (currentTime <= 0) 
            {
                _debug.Log("La carga llego a 0");
                isCharging = false;
            } 
        }
    }

    //condicion de disparo
    public override bool ShootCondition() => _currentCooldown <= 0;
    
}
