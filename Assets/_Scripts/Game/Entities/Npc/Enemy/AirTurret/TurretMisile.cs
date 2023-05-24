using FacundoColomboMethods;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(LifeComponent))]
public class TurretMisile : MonoBehaviour
{
    [System.Serializable]
    public struct MisileStats
    {
        public Enemy_AirTurret owner;
        public int damage;
       

        public float life;

        [Header("Explosion")]
        public float explosionRadius;
        [Tooltip("La fuerza que le aplica a otros rigidboy al explotar(Varia segun la distancia a la que el misil explote)")]
        public float explosionForce;
        [Tooltip("La fuerza hacia arriba que se le aplica al objeto")]
        public float upwardsMultiplier;
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

    Vector3 _upwardsMultiplier;

    DebugableObject _debug;
    Rigidbody _rb;
    Action _movement;
    LifeComponent _lifeComponent;
    private void Awake()
    {
        _lifeComponent = GetComponent<LifeComponent>();
        _lifeComponent.OnKilled += Explosion;

        _rb = GetComponent<Rigidbody>();

        _debug = GetComponent<DebugableObject>();
        _debug.AddGizmoAction(MisileGizmos);
    }

    public void Initialize(MisileStats _myNewStats,Transform _newTarget,Vector3 forward)
    {
        _lifeComponent.SetNewMaxLife((int)_myStats.life); 
        _myStats = _myNewStats;
        _target = _newTarget;
        transform.forward = forward;
        _movement = MoveForward;

        // Calcular fuerza hacia arriba para no tener que hacerlo cada frame.
        _upwardsMultiplier = Vector3.up * _myStats.upwardsMultiplier;

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
        transform.forward += steering.normalized * Time.deltaTime * _myStats.steeringSpeed * steering.magnitude;

        if (_myStats.stopTrackingRadius > desiredDir.magnitude)
        {
            _movement -= TrackTarget;
            transform.forward = desiredDir.normalized;
        }
      
    }


    IEnumerator ChangeCourse()
    {
        yield return new WaitForSeconds(_myStats.timeBeforeTracking);
        //_myStats.owner = null;
        _rb.velocity = _rb.velocity / 1.5f ;
        _movement = null;
        _movement += TrackTarget;
        _movement += MoveForward;
     

    }

    #endregion

    #region Explosion Logic
    void Explosion()
    {
        foreach (IDamagable x in transform.position.GetItemsOFTypeAround<IDamagable>(_myStats.explosionRadius).Where(x => x != this))
            x.TakeDamage(_myStats.damage);


        foreach (Rigidbody rb in transform.position.GetItemsOFTypeAround<Rigidbody>(_myStats.explosionRadius).Where(x => x != this)) {

            Vector3 vector = rb.position - transform.position;
            // Conseguimos la direccion en el plano XZ
            Vector3 dirXZ = Vector3.ProjectOnPlane(vector, Vector3.up).normalized;
            // Calculamos la fuerza de la explosion en base a la distancia al objeto
            float explosionForce = (1 - vector.magnitude / _myStats.explosionRadius) *_myStats.explosionForce;
            _debug.Log("EXPLOSION FORCE: " + explosionForce);
            rb.AddForce((dirXZ + _upwardsMultiplier) * explosionForce, ForceMode.Impulse);
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


    private void MisileGizmos()
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

 

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy_AirTurret>() == _myStats.owner) return;

        Explosion();
    }
}
