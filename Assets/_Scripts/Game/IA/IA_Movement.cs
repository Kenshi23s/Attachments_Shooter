using FacundoColomboMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#region ComponentsRequired
[RequireComponent(typeof(FOVAgent))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(Physics_Movement))]

#endregion

public class IA_Movement : MonoBehaviour
{

    DebugableObject _debug;
    public LayerMask obstacleMask; 

    FOVAgent _fov;
    Physics_Movement movement;

    Vector3 velocity => movement.velocity;
    [Header("Flocking Forces")]

    [SerializeField,Range(0f,3f)]  float _alignmentForce;
    [SerializeField,Range(0f, 3f)] float _cohesionForce;
    [SerializeField,Range(0f, 3f)] float _separationForce;

    Action _fixedUpdate;

 
    private void Awake()
    {
        _fov = GetComponent<FOVAgent>();
        movement = GetComponent<Physics_Movement>();
    }

    private void FixedUpdate() => _fixedUpdate?.Invoke();

    #region Pathfinding Methods
    public void SetDestination(Vector3 target)
    {
        if (transform.position.InLineOffSight(target,IA_Manager.instance.wall_Mask))
        {
            _fixedUpdate = () => 
            {
                _debug.Log("veo el destino, voy directo ");

                Vector3 actualForce = Vector3.zero;
                actualForce += Seek(target);
                actualForce += FlockingUpdate();
                actualForce += ObstacleAvoidance(transform);
                movement.AddForce(actualForce);
            };
        }
        else
        {
            _debug.Log( "NO veo el destino, Calculo el camino ");

            List<Vector3> waypoints = SetPath(target);
            _fixedUpdate = () => PlayPath(waypoints);
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
            _fixedUpdate = null;           
            return;
        }

        _debug.Log("Se mueve hacia el siguiente nodo, me faltan " + waypoints.Count);

        Vector3 actualForce = Vector3.zero;

        actualForce += Seek(waypoints[0]);
        actualForce += FlockingUpdate();
        actualForce += ObstacleAvoidance(transform);

        movement.AddForce(actualForce);
        if (Vector3.Distance(waypoints[0], transform.position) < 2f) waypoints.RemoveAt(0);
    }    

    #endregion

    #region Movement Updates
    public Vector3 FlockingUpdate()
    {           
       Vector3 actualforce = Vector3.zero;
        _debug.Log("Flocking");
       
        actualforce += Alignment() * _alignmentForce;
        actualforce += Cohesion() * _cohesionForce;
        actualforce += Separation() * _separationForce;

        return actualforce;     
    }

   

        //force += ObstacleAvoidance(_rb.transform);
   

    Vector3 ObstacleAvoidance(Transform transform)
    {

        float dist = velocity.magnitude;

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
            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= _fov.viewRadius)
            {
                desired += item.velocity;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;

        desired.Normalize();
        desired *= movement.maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in IA_Manager.instance.flockingTargets.Where(x => x != this))
        {
            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= _fov.viewRadius)
            {
                desired += item.transform.position;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;
        desired -= transform.position;

        desired.Normalize();
        desired *= movement.maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;
        IEnumerable<IA_Movement> targets = IA_Manager.instance.flockingTargets.Where(x => x != this);
        foreach (var item in targets)
        {        
            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= _fov.viewRadius)
                desired += dist;
        }

        if (desired == Vector3.zero)
            return desired;

        desired = -desired;

        desired.Normalize();
        desired *= movement.maxForce;

        return CalculateSteering(desired);
    }
    #endregion

    #region MovementTypes
    public Vector3 Seek(Vector3 targetSeek)
    {
    
        Vector3 desired = targetSeek - transform.position;
        desired.Normalize();
        desired *= movement.maxForce;

        return CalculateSteering(desired); ;
    }


    public Vector3 Arrive(Vector3 actualTarget,float arriveRadius)
    {
       
       
        Vector3 desired = actualTarget - transform.position;
        float dist = desired.magnitude;
        desired.Normalize();
        if (dist <= arriveRadius)
            desired *= movement.maxSpeed * (dist / arriveRadius);
        else
            desired *= movement.maxSpeed;   
        return CalculateSteering(desired);
    }

    #endregion

    Vector3 CalculateSteering(Vector3 desired) => Vector3.ClampMagnitude(desired - velocity, movement.maxSpeed);

  
}
