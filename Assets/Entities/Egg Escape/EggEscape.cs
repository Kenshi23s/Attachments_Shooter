using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EggEscapeModel : Entity
{
    NavMeshAgent agent;

    StateMachine<EggStates> _fsm;

    public enum EggStates
    {
        Patrol,
        Escape,
        Kidnapped
    }
    //tendria que tener 3 estados:
    // Patrullar, hago la mia y sigo los waypoints, trato de no hacer el mismo camino q otro huevo en caso q haya
    // Escapar: vi al player con FOV al patrullar asi q me re tomo el palo en la direccion contraria a la q viene
    // Liberarse: el player me agarro, desp de x segundos me libero y paso directo a el estado escapar
    private void Awake()
    {
        agent=GetComponent<NavMeshAgent>();
    }

    public void Initialize(Vector3 SpawnPos)
    {
        transform.position = SpawnPos;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Execute();
    }

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    public override void Resume()
    {
        throw new System.NotImplementedException();
    }

    public override int TakeDamage(int dmgDealt)
    {
        throw new System.NotImplementedException();
    }

    public override bool WasCrit() => false;
   

    public override bool WasKilled()=> false;
}
