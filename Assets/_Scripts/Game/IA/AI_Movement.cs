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

public class AI_Movement : MonoBehaviour
{
    DebugableObject _debug;
    public FOVAgent FOV { get; private set; }
    public Physics_Movement Movement { get; private set; }

    public Vector3 Destination { get; private set; } 

    public Vector3 Velocity => Movement._velocity;

    [Header("Flocking Forces")]

    public LayerMask WhatIsGround;
    public LayerMask ObstacleMask;
    Action _fixedUpdate;

    public event Action OnDestinationReached, OnDestinationChanged, OnMovementCanceled;

    Vector3 _destination;
    List<Vector3> _waypoints = new List<Vector3>();

    #region Flocking
    [SerializeField]
    public bool Flocking {get; private set;}
    FlockingParameters _flockingParameters = new FlockingParameters();
    [SerializeField]
    IEnumerable<AI_Movement> _flockingTargets = default;
    #endregion

    private void Awake()
    {
        FOV = GetComponent<FOVAgent>();
        Movement = GetComponent<Physics_Movement>();
        _debug = GetComponent<DebugableObject>();    

        _flockingParameters.myTransform = transform;
        _flockingParameters.maxForce = Movement.maxForce;
        _flockingParameters.viewRadius= FOV.viewRadius;
    }

    public void SetTargets(IEnumerable<AI_Movement> targets)
    {
        _debug.Log("Targets para flocking Seteados");
        _flockingTargets = targets;
    }

    private void FixedUpdate() => _fixedUpdate?.Invoke();

    public void SetMaxSpeed(float newSpeed) =>  Movement.maxSpeed = newSpeed;
      
    #region Pathfinding Methods
    /// <summary>
    /// Se mueve hasta esa ubicacion usando Theta* (aplicando flocking y obstacle avoidance)
    /// Si la ubicacion esta a la vista no calcula el camino y va directo
    /// </summary>
    /// <param name="target"></param>
    public void SetDestination(Vector3 target)
    {
        if (_waypoints.Any() && target != _destination) OnDestinationChanged?.Invoke();

        _destination = target;        

        if (transform.position.InLineOffSight(target,IA_Manager.instance.wall_Mask))
        {
            _fixedUpdate = () => 
            {
                _debug.Log("Veo el destino, voy directo.");
                MoveTowards(target);
                if (Vector3.Distance(target, transform.position) < 2f) { 
                    OnDestinationReached?.Invoke();
                    ClearPath();
                }
            };
        }
        else
        {
            _debug.Log( "No veo el destino, calculo el camino.");

            _waypoints = GetPath(target);
            _fixedUpdate = PlayPath;
        }
    }

   

    void MoveTowards(Vector3 target)
    {
        Vector3 actualForce = Vector3.zero;
        actualForce += Movement.Seek(target);
        if (Flocking) actualForce += _flockingTargets.Flocking(_flockingParameters);
        actualForce += ObstacleAvoidance(transform);
        actualForce = ProjectAlongSlope(actualForce);
        
        Movement.AddForce(Velocity.CalculateSteering(actualForce, Movement.maxSpeed));
    }

    public Vector3 ProjectAlongSlope(Vector3 force) 
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 0.5f, WhatIsGround))
        {
            force = Vector3.ProjectOnPlane(force, hit.normal);
        }

        return force;
    }

    /// <summary>
    /// Consigue un camino hacia un objetivo usando Theta*
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    List<Vector3> GetPath(Vector3 target)
    {
        IA_Manager I = IA_Manager.instance;
        
        Tuple<Node, Node> keyNodes = Tuple.Create(I.GetNearestNode(transform.position), I.GetNearestNode(target));
        
        return keyNodes.CalculateThetaStar(I.wall_Mask, target);   
    }

    /// <summary>
    /// Reproduce el camino con Theta*
    /// </summary>
    /// <param name="waypoints"></param>
    void PlayPath()
    {
        if (!_waypoints.Any()) 
        {
            OnDestinationReached?.Invoke();
            ClearPath();
            return;
        }

        _debug.Log("Se mueve hacia el siguiente nodo, me faltan " + _waypoints.Count);

        MoveTowards(_waypoints[0]);

        if (Vector3.Distance(_waypoints[0], transform.position) < 2f) _waypoints.RemoveAt(0);
    }


    void ClearPath()
    {
        _fixedUpdate = null;
        _waypoints.Clear();
    }

    public void CancelMovement() 
    {
        ClearPath();
        OnMovementCanceled?.Invoke();
    }
    #endregion

    #region Movement Updates 

    Vector3 ObstacleAvoidance(Transform transform)
    {

        float dist = Velocity.magnitude;

        if (Physics.SphereCast(transform.position, 1.5f, transform.forward, out RaycastHit hit, dist, ObstacleMask))
        {
            _debug.Log("ESTOY HACIENDO OBSTACLE AVOIDANCE!");
            Vector3 obstacle = hit.transform.position;
            Vector3 dirToObject = obstacle - transform.position;
            float angleInBetween = Vector3.SignedAngle(transform.forward, dirToObject, Vector3.up);

            Vector3 desired = angleInBetween >= 0 ? -transform.right : transform.right;

            return Velocity.CalculateSteering(desired,Movement.maxSpeed);
        }

        return Vector3.zero;
    }

    #endregion

    
}
