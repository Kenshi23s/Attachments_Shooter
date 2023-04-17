using FacundoColomboMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class TurretMisile : MonoBehaviour
{
    public struct MisileStats
    {
        public int damage;


        public float life;

        public float explosionRadius;
        public float explosionForce;
        public float triggerRadius;


        public float force;
        public float maxSpeed;
        public float timeBeforeTracking;
        public float stopTrackingRadius;
        public float steeringSpeed;




    }
   
    MisileStats _myStats;
    Transform _target;
    Rigidbody _rb;
    Action _movement;

    public void Initialize(MisileStats _myNewStats,Transform _newTarget)
   {
        this._myStats = _myNewStats;
        this._target = _newTarget;
        _movement = MoveForward;
        StartCoroutine(ChangeCourse());
   }

    #region Updates
    private void FixedUpdate() => _movement?.Invoke();
  
    void LateUpdate()
    {
        if (_myStats.triggerRadius > Vector3.Distance(_target.transform.position,transform.position))
            Explosion();
    }
    #endregion
 
    #region Movement Logic

    void MoveForward() => _rb.velocity += Vector3.ClampMagnitude(transform.forward * Time.deltaTime * _myStats.force, _myStats.maxSpeed);

    void TrackTarget()
    {
        transform.forward = (_target.position - transform.forward) * Time.deltaTime * _myStats.steeringSpeed;
        if (_myStats.stopTrackingRadius > Vector3.Distance(transform.position, _target.position))
        {
            _movement -= TrackTarget;
        }
      
    }
    IEnumerator ChangeCourse()
    {
        yield return new WaitForSeconds(_myStats.timeBeforeTracking);
        _movement += TrackTarget;
    }

    #endregion

    #region Damagable

    public int TakeDamage(int dmgDealt)
    {
        _myStats.life -= dmgDealt;
        if (_myStats.life<=0)
        {
            Explosion();
        }
        return dmgDealt;
    }

    public bool WasCrit() => false;

    public bool WasKilled() => _myStats.life <= 0;
    #endregion

    #region Explosion Logic
    void Explosion()
    {
        foreach (IDamagable x in transform.position.GetItemsOFTypeAround<IDamagable>(_myStats.explosionRadius))
            x.TakeDamage(_myStats.damage);

        foreach (Rigidbody rb in transform.position.GetItemsOFTypeAround<Rigidbody>(_myStats.explosionRadius))
            rb.AddExplosionForce(GetForce(rb.position), transform.position, _myStats.explosionRadius);

        _movement = null;

        //devolver a la pool de proyectiles enemigos, feedback, etc
    }

    float GetForce(Vector3 target)
    {
        // la distancia / el radio
        // pongo 1 - (valor entre 0 y 1) porque:
        // si esta en el limite de la distancia, la division me daria 1, por lo que recibiria la maxima fuerza si no lo restara por -1
        // y por si estoy en el centro, la division seria " 0 / 1" y en caso de que no le restara -1, no recibiria la maxima fuerza
        // deberia informarme mas de matematica para videojuegos, sin jocha no podria haber sacado esta "Magnitud Inversa"
        float divide = (1 - Vector3.Distance(transform.position, target) / _myStats.explosionRadius);
        return divide * _myStats.explosionForce;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (transform.forward * _myStats.maxSpeed) + transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position,(transform.forward * _myStats.force) + transform.position);

        Gizmos.DrawWireSphere(transform.position, _myStats.explosionRadius);
        

    }

}
