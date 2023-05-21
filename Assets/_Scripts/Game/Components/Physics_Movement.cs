using System.Collections;
using UnityEngine;

#region Components
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(PausableObject))]
#endregion
[DisallowMultipleComponent]
//se usa para hacer movimiento "Realista"(lo usan las IA del juego, pero no esta limitado a ellas)
public class Physics_Movement : MonoBehaviour
{
    Rigidbody _rb;
    DebugableObject _debug;

   
    public Vector3 _velocity { get; private set; }

    [SerializeField,Range(1,100)]
    float _maxForce;
    public float maxForce => _maxForce;

    [SerializeField,Range(1,100)]
    float _maxSpeed;
    public float maxSpeed => _maxSpeed;

    [SerializeField, Range(0.1f, 10)]
    float _steeringForce;
    public float steeringForce => _steeringForce;



    private void Awake()
    {
        GetComponent<PausableObject>().onPause += () => StartCoroutine(OnPause());

        _debug = GetComponent<DebugableObject>(); _debug.AddGizmoAction(MovementGizmos);
        _rb = GetComponent<Rigidbody>();
    }

    public void RemoveForces()
    {
        _rb.velocity = Vector3.zero;
        _debug.Log("Se removieron todas las fuerzas");
    }

    public void AddForce(Vector3 force)
    {      
        _velocity = Vector3.ClampMagnitude(_velocity + force * Time.fixedDeltaTime, _maxForce);

        _rb.velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

        transform.forward = _velocity.normalized;
    }

    public void SetMaxSpeed(float _maxSpeed)
    {
        _debug.Log($"MAXSPEED cambio de {this._maxSpeed} a {_maxSpeed}");
        this._maxSpeed = _maxSpeed;
    }

    public void SetMaxForce(float _maxForce)
    {
        _maxForce = Mathf.Clamp(_maxForce, 1, _maxSpeed);
        _debug.Log($"MAXSPEED cambio de {this._maxForce} a {_maxForce}");
        this._maxForce = _maxForce;
    }

    IEnumerator OnPause()
    {
        Vector3 actualVelocity = _rb.velocity;
        _rb.velocity = Vector3.zero;
        yield return new WaitWhile(() => ScreenManager.isPaused);
        _rb.velocity = actualVelocity;
    }
 
    void MovementGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rb.position, _rb.position + _velocity);
    }

   public Vector3 CalculateSteering(Vector3 desired) => Vector3.ClampMagnitude(desired - _velocity * steeringForce, maxSpeed);
}
