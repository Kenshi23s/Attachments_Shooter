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
    [SerializeField]
    FlockingParameters flockingParameters= new FlockingParameters();
    DebugableObject _debug;
    FOVAgent _fov;
    Physics_Movement movement;

    public Vector3 destination { get; private set; } 

    public Vector3 velocity => movement._velocity;

    [Header("Flocking Forces")]

    public LayerMask obstacleMask;
    Action _fixedUpdate;

    [SerializeField]public bool isFlockingActive {get; private set;}
   
    private void Awake()
    {
        _fov = GetComponent<FOVAgent>();
        movement = GetComponent<Physics_Movement>();
        _debug = GetComponent<DebugableObject>();    

        flockingParameters.myTransform = transform;
        flockingParameters.maxForce = movement.maxForce;
        flockingParameters.viewRadius= _fov.viewRadius;
    }

    private void FixedUpdate() => _fixedUpdate?.Invoke();

    public void SetMaxSpeed(float newSpeed) =>   movement.SetMaxSpeed(newSpeed);
      
    #region Pathfinding Methods
    /// <summary>
    /// Se mueve hasta esa ubicacion usando TITA*(aplicando flocking y obstacle avoidance)
    /// si la ubicacion esta a la vista no calcula el camino y va directo
    /// </summary>
    /// <param name="target"></param>
    public void SetDestination(Vector3 target)
    {
        if (transform.position.InLineOffSight(target,IA_Manager.instance.wall_Mask))
        {
            _fixedUpdate = () => 
            {
                _debug.Log("veo el destino, voy directo ");

                Vector3 actualForce = Vector3.zero;
                actualForce += Seek(target);
                actualForce += IA_Manager.instance.flockingTargets.Flocking(flockingParameters);
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
    /// <summary>
    /// setea el camino para TITA*
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    List<Vector3> SetPath(Vector3 target)
    {
        IA_Manager I = IA_Manager.instance;
    
        Tuple<Node, Node> keyNodes = Tuple.Create(I.GetNearestNode(transform.position), I.GetNearestNode(target));
    
        return keyNodes.CalculateThetaStar(I.wall_Mask, target);   
    }
    /// <summary>
    /// Reproduce el camino con TITA*
    /// </summary>
    /// <param name="waypoints"></param>
    void PlayPath(List<Vector3> waypoints)
    {
        if (!waypoints.Any()) 
        {
            _fixedUpdate = null;
            movement.RemoveForces();
            return;
        }

        _debug.Log("Se mueve hacia el siguiente nodo, me faltan " + waypoints.Count);

        Vector3 actualForce = Vector3.zero;

        actualForce += Seek(waypoints[0]);
        actualForce += IA_Manager.instance.flockingTargets.Flocking(flockingParameters); 
        actualForce += ObstacleAvoidance(transform);

        
        movement.AddForce(velocity.CalculateSteering(actualForce, movement.steeringForce));
        if (Vector3.Distance(waypoints[0], transform.position) < 2f) waypoints.RemoveAt(0);
    }    

    #endregion

    #region Movement Updates 

    Vector3 ObstacleAvoidance(Transform transform)
    {

        float dist = velocity.magnitude;

        if (Physics.SphereCast(transform.position, 1.5f, transform.forward, out RaycastHit hit, dist, obstacleMask))
        {
            Vector3 obstacle = hit.transform.position;
            Vector3 dirToObject = obstacle - transform.position;
            float angleInBetween = Vector3.SignedAngle(transform.forward, dirToObject, Vector3.up);

            Vector3 desired = angleInBetween >= 0 ? -transform.right : transform.right;

            return velocity.CalculateSteering(desired,movement.maxSpeed);
        }

        return Vector3.zero;
    }
    #endregion

    #region MovementTypes
    public Vector3 Seek(Vector3 targetSeek)
    {  
        Vector3 desired = targetSeek - transform.position;
        desired.Normalize();
        return desired* movement.maxForce;
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
        return desired;
    }
    #endregion  
}
