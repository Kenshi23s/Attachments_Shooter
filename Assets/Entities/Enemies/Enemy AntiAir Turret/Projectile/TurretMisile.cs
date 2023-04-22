using FacundoColomboMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class TurretMisile : MonoBehaviour,IDetector
{
    [System.Serializable]
    public struct MisileStats
    {
        public int damage;


        public float life;

        [Header("Explosion")]
        public float explosionRadius;
        [Tooltip("La fuerza que le aplica a otros rigidboy al explotar(Varia segun la distancia a la que el misil explote)")]
        public float explosionForce;
        [Tooltip("La distancia necesaria para explotar"), Range(0, 7)]
        public float triggerRadius;

        [Header("Movement")]
        [Range(0,100),Tooltip("la fuerza no deberia ser mayor a la velocidad maxima")] 
        public float force;
        [Range(0, 100)] public float maxSpeed;

        [Header("Tracking")]
        public float steeringSpeed;
        [Tooltip("Tiempo antes de empezar a trackear al objetivo"),Range(0,7)]
        public float timeBeforeTracking;
        [Tooltip("La distancia a la que el misil deja de trackear(Por estar cerca del target)"), Range(0, 100)]
        public float stopTrackingRadius;
     




    }
   
    [SerializeField,Tooltip("La torreta pasa estos parametros")]
     MisileStats _myStats;
    [SerializeField]
    Transform _target;

    Rigidbody _rb;
    Action _movement;

    private void Awake() => _rb = GetComponent<Rigidbody>();
  
    public void Initialize(MisileStats _myNewStats,Transform _newTarget,Vector3 forward)
    {
        this._myStats = _myNewStats;
        this._target = _newTarget;
        transform.forward = forward;
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

    void MoveForward()
    {
        Vector3 fwd = transform.forward * Time.deltaTime * _myStats.force;
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + fwd, _myStats.maxSpeed);
    } 

    void TrackTarget()
    {
        Vector3 desiredDir = _target.position - transform.position;
        Vector3 steering = desiredDir - _rb.velocity;
        transform.forward += steering.normalized * Time.deltaTime * _myStats.steeringSpeed* steering.magnitude;

        if (_myStats.stopTrackingRadius > desiredDir.magnitude)
        {
            _movement -= TrackTarget;
            transform.forward = desiredDir.normalized;
        }
      
    }


    IEnumerator ChangeCourse()
    {
        yield return new WaitForSeconds(_myStats.timeBeforeTracking);
        _rb.velocity = _rb.velocity / 1.5f ;
        _movement = null;
        _movement += TrackTarget;
        _movement += MoveForward;
     

    }

    #endregion

    #region Damagable

    public int TakeDamage(int dmgDealt)
    {
        _myStats.life -= dmgDealt/2;
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
        {
            x.TakeDamage(_myStats.damage);
        }
           

        foreach (Rigidbody rb in transform.position.GetItemsOFTypeAround<Rigidbody>(_myStats.explosionRadius))
        {
           
            rb.AddExplosionForce(GetForce(rb.position), transform.position, _myStats.explosionRadius);
        }
           

        _movement = null;

        //esto sacarlo y meterlo en una pool mas adelante
        Destroy(this.gameObject);

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
        if (_target == null) return;
        

        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (transform.forward * _myStats.maxSpeed) + transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position,(transform.forward * _myStats.force) + transform.position);

        Gizmos.DrawWireSphere(transform.position, _myStats.explosionRadius);
        if (_target!=null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _target.position);
            Gizmos.DrawWireSphere(_target.position,3f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_target.position, _myStats.stopTrackingRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_target.position, _myStats.explosionRadius);



    }

    public void OnRangeCallBack(Player_Movement item)
    {
        throw new NotImplementedException();
    }

    public void OutOfRangeCallBack(Player_Movement item)
    {
        throw new NotImplementedException();
    }
}
