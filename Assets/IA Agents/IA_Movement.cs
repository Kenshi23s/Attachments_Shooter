using FacundoColomboMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#region ComponentsRequired
[RequireComponent(typeof(FOVAgent))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(Rigidbody))]
#endregion

public class IA_Movement : MonoBehaviour
{

    DebugableObject _debug;
    public LayerMask obstacleMask;

    private Vector3 _velocity;
    public Vector3 Velocity => _velocity; 

    Rigidbody _rb;
    [SerializeField, Range(0f, 50f)]  float _maxSpeed;
    [SerializeField, Range(0f, 50f)]  float _maxForce;
    FOVAgent _fov;


    [Header("Forces")]

    [SerializeField,Range(0f,3f)]  float _alignmentForce;
    [SerializeField,Range(0f, 3f)] float _cohesionForce;
    [SerializeField,Range(0f, 3f)] float _separationForce;

    Action _update;

 
    private void Awake()
    {
        _fov = GetComponent<FOVAgent>();
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        _rb.useGravity  = false;
        _debug = GetComponent<DebugableObject>();
        _debug.AddGizmoAction(MovementGizmos);
    }

    private void FixedUpdate() => _update?.Invoke();

    #region Pathfinding Methods
    public void SetDestination(Vector3 target)
    {

        if (transform.position.InLineOffSight(target,IA_Manager.instance.wall_Mask))
        {
            _update = () => 
            {
                _debug.Log("veo el destino, voy directo ");
                Vector3 actualForce = Vector3.zero;
                actualForce += Seek(target);
                actualForce += FlockingUpdate();
                AddForce(actualForce);

            };
        }
        else
        {
            _debug.Log( "NO veo el destino, Calculo el camino ");
            List<Vector3> waypoints = SetPath(target);
            _update = () => PlayPath(waypoints);
          

        }
       
    }
    
    List<Vector3> SetPath(Vector3 target)
    {
        IA_Manager I = IA_Manager.instance;
    
        Tuple<Node, Node> keyNodes = Tuple.Create(I.GetNearestNode(transform.position), I.GetNearestNode(target));
    
        return keyNodes.CalculateThetaStar(I.wall_Mask, target);
    
    
    }

    void PlayPath(List<Vector3> waypoints)
    {
        if (!waypoints.Any()) 
        {
            _update = null;
            StopMoving();
            return;
        }

        _debug.Log("Se mueve hacia el siguiente nodo, me faltan " + waypoints.Count);

        Vector3 actualForce = Vector3.zero;
        actualForce += Seek(waypoints[0]);
        actualForce += FlockingUpdate();
        AddForce(actualForce);
        if (Vector3.Distance(waypoints[0], transform.position) < 2f) waypoints.RemoveAt(0);
    }

    void StopMoving()
    {
        _rb.velocity = Vector3.zero;
    }

    #endregion

    #region Movement Updates
    public Vector3 FlockingUpdate()
    {    
        
       Vector3 actualforce = Vector3.zero;
        _debug.Log("Flocking On" + gameObject.name);
       
        actualforce += Alignment() * _alignmentForce;
        actualforce += Cohesion() * _cohesionForce;
        actualforce += Separation() * _separationForce;

        return actualforce;
       //AddForce(actualforce);
        
       

    }

   

    public void AddForce(Vector3 force)
    {
        force += ObstacleAvoidance(_rb.transform);

        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxForce);

        _rb.velocity = Vector3.ClampMagnitude(_velocity,_maxSpeed);

        _rb.transform.forward = Velocity.normalized;
       

    }

    Vector3 ObstacleAvoidance(Transform transform)
    {

        float dist = Velocity.magnitude;

        if (Physics.SphereCast(transform.position, 1, transform.forward, out RaycastHit hit, dist, obstacleMask))
        {
            Vector3 obstacle = hit.transform.position;
            Vector3 dirToObject = obstacle - transform.position;
            float angleInBetween = Vector3.SignedAngle(transform.forward, dirToObject, Vector3.up);

            Vector3 desired = angleInBetween >= 0 ? -transform.right : transform.right;

            return CalculateSteering(desired);
        }

        return Vector3.zero;
    }
    #endregion

    #region Flocking
    Vector3 Alignment()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (IA_Movement item in IA_Manager.instance.flockingTargets.Where(x => x != this))
        {
            Vector3 dist = item.transform.position - _rb.position;

            if (dist.magnitude <= _fov.viewRadius)
            {
                desired += item.Velocity;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;

        desired.Normalize();
        desired *= _maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in IA_Manager.instance.flockingTargets.Where(x => x != this))
        {
           

            Vector3 dist = item.transform.position - _rb.position;

            if (dist.magnitude <= _fov.viewRadius)
            {
                desired += item.transform.position;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;
        desired -= _rb.position;

        desired.Normalize();
        desired *= _maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in IA_Manager.instance.flockingTargets.Where(x => x != this))
        {
           
            
            Vector3 dist = item.transform.position - _rb.position;

            if (dist.magnitude <= _fov.viewRadius)
                desired += dist;
        }

        if (desired == Vector3.zero)
            return desired;

        desired = -desired;

        desired.Normalize();
        desired *= _maxForce;

        return CalculateSteering(desired);
    }
    #endregion

    #region MovementTypes
    public Vector3 Seek(Vector3 targetSeek)
    {
    
        Vector3 desired = targetSeek - _rb.position;
        desired.Normalize();
        desired *= _maxForce;

        Vector3 steering = desired - Velocity;

        steering = Vector3.ClampMagnitude(steering, _maxSpeed);
        
        return steering;
    }


    public Vector3 Arrive(Vector3 actualTarget,float arriveRadius)
    {
       
        Debug.Log("Arrive");
        Vector3 desired = actualTarget - _rb.position;
        float dist = desired.magnitude;
        desired.Normalize();
        if (dist <= arriveRadius)
            desired *= _maxSpeed * (dist / arriveRadius);
        else
            desired *= _maxSpeed;

        //Steering
        Vector3 steering = desired - Velocity;
      
        return steering;
    }

    #endregion

    Vector3 CalculateSteering(Vector3 desired) => Vector3.ClampMagnitude(desired - _velocity, _maxSpeed);

    public void MovementGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rb.position, _rb.position + _velocity);

    }

  
}
