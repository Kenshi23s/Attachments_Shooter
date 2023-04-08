using FacundoColomboMethods;
using UnityEngine;
using static EggEscapeModel;

public class EggPatrolState:IState
{
    Transform actualWaypoint;

    // lo deberia tener la clase de gamemode (?)
    Transform player;

    //se debe pasar por constructor

    EggStates_Var _eggFSM_Variables;

    public EggPatrolState(EggStates_Var _eggFSM_Variables)
    {
        this._eggFSM_Variables = _eggFSM_Variables;
    }

    public void OnEnter() => actualWaypoint = GetRandomWaypoint();

    public void OnUpdate()
    {
        _eggFSM_Variables.agent.SetDestination(actualWaypoint.position);
        float distance = Vector3.Distance(actualWaypoint.position, _eggFSM_Variables.agent.transform.position);

        if (distance < _eggFSM_Variables.gameMode.interactRadius)
            actualWaypoint = GetRandomWaypoint();
    }

    Transform GetRandomWaypoint() => 
    _eggFSM_Variables.gameMode.waypoints[Random.Range(0, _eggFSM_Variables.gameMode.waypoints.Length - 1)];

    public void MakeDecision() 
    {
        if (_eggFSM_Variables.fov.inFOV(_eggFSM_Variables.gameMode.playerPos)) 
            _eggFSM_Variables._fsm.ChangeState(EggStates.Escape);
    }

    public void OnExit() { }
 

    public void GizmoState()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(actualWaypoint.position, _eggFSM_Variables.gameMode.interactRadius);
    }
    //Node RandomNodeChange()
    //{
    //    //talvez deberia ver de optimizar el metodo(?)

    //    // si los vecinos del nodo son mayor a 1
    //    if (actualPatrolNode.Neighbors.Count > 1)
    //    {
    //        //selecciono alguno de esos 2
    //        Node newNode = actualPatrolNode.Neighbors[UnityEngine.Random.Range(0, actualPatrolNode.Neighbors.Count)];
    //        //si no es el nodo q patrulle antes
    //        if (newNode != lastPatrolNode)
    //        {
    //            //y un agente no paso por ahi hace poco
    //            if (newNode.agentNear != true)
    //            {
    //                //lo obtengo y remplazo el nodo anterior por el actual, y el actual por el nuevo
    //                lastPatrolNode = actualPatrolNode;
    //                return newNode;
    //            }
    //            else
    //            {
    //                //sino, busco entre los vecinos de  el nodo
    //                foreach (Node neighbor in actualPatrolNode.Neighbors)
    //                {
    //                    //si el vecino no es el nodo q ya habia chequeado y no hubo un agente por ahi
    //                    if (neighbor != newNode && neighbor.agentNear != true)
    //                    {
    //                        //lo obtengo y remplazo el nodo anterior por el actual, y el actual por el nuevo
    //                        lastPatrolNode = actualPatrolNode;
    //                        return neighbor;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {   //sino, aplico recursion y llamo de nuevo al metodo
    //            return RandomNodeChange();
    //        }
    //    }
    //    //sino, por default devuelvo el vecino q este en el indice 0
    //    lastPatrolNode = actualPatrolNode;
    //    return actualPatrolNode.Neighbors[0];


    //}
}
public class EggEscapeState:IState
{
    Transform actualWaypoint;
    EggStates_Var _eggFSM_Var;

    public EggEscapeState(EggStates_Var eggFSM_Var) => _eggFSM_Var = eggFSM_Var;

    Transform GetFurthestWaypoint() => _eggFSM_Var.gameMode.waypoints.GetFurthest(_eggFSM_Var.agent.transform.position);

    public void OnEnter()
    {
        if (!_eggFSM_Var.fov.inFOV(_eggFSM_Var.gameMode.playerPos))
           { _eggFSM_Var._fsm.ChangeState(EggStates.Patrol); return;}
                 
          actualWaypoint=GetFurthestWaypoint();
    }  
   
    public void OnUpdate() => _eggFSM_Var.agent.SetDestination(actualWaypoint.position);

    public void MakeDecision()
    {
        float distance = Vector3.Distance(actualWaypoint.position, _eggFSM_Var.agent.transform.position);
        if (distance < _eggFSM_Var.gameMode.interactRadius)
        {
            if (_eggFSM_Var.fov.inFOV(_eggFSM_Var.gameMode.playerPos))
            {
                _eggFSM_Var._fsm.ChangeState(EggStates.Patrol);
                return;
            }
            actualWaypoint = GetFurthestWaypoint();        
        }           
        
    }
    public void GizmoState()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(actualWaypoint.position, _eggFSM_Var.gameMode.interactRadius);
    }

    public void OnExit() { }
}
public class EggKidnapedState:IState
{
    EggStates_Var _eggFSM_Variables;

    public void GizmoState()
    {
        throw new System.NotImplementedException();
    }

    public void MakeDecision()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

   

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

