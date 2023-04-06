using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EggEscapeModel;

public class EggPatrolState:IState
{
    Node actualPatrolNode,lastPatrolNode;

    // lo deberia tener la clase de gamemode (?)
    Transform player;

    //se debe pasar por constructor
    StateMachine<EggStates> _fsm;
    NavMeshAgent agent;
    FOVAgent fov;
    EggGameChaseMode _gameMode;
  

    float interactRadius;


    public void OnEnter()
    {
        actualPatrolNode = ColomboMethods.GetNearest(_gameMode.nodes,agent.transform.position);
    }

    public void OnUpdate()
    {
        agent.SetDestination(actualPatrolNode.position);
        if (Vector3.Distance(actualPatrolNode.position,agent.transform.position)<interactRadius)
        {
            //el metodo ya cambia el nodo actual a viejo
            actualPatrolNode = RandomNodeChange();
        }
    }


    public void OnExit()
    {
        actualPatrolNode.ChangeNear(false);
    }

    public void MakeDecision()
    {
        if (fov.inFOV(player.position))
        {
            _fsm.ChangeState(EggEscapeModel.EggStates.Escape);
        }
    }

   
    public void GizmoState()
    {

    }
    Node RandomNodeChange()
    {
        //talvez deberia ver de optimizar el metodo(?)

        // si los vecinos del nodo son mayor a 1
        if (actualPatrolNode.Neighbors.Count > 1)
        {
            //selecciono alguno de esos 2
            Node newNode = actualPatrolNode.Neighbors[UnityEngine.Random.Range(0, actualPatrolNode.Neighbors.Count)];
            //si no es el nodo q patrulle antes
            if (newNode != lastPatrolNode)
            {
                //y un agente no paso por ahi hace poco
                if (newNode.agentNear != true)
                {
                    //lo obtengo y remplazo el nodo anterior por el actual, y el actual por el nuevo
                    lastPatrolNode = actualPatrolNode;
                    return newNode;
                }
                else
                {
                    //sino, busco entre los vecinos de  el nodo
                    foreach (Node neighbor in actualPatrolNode.Neighbors)
                    {
                        //si el vecino no es el nodo q ya habia chequeado y no hubo un agente por ahi
                        if (neighbor != newNode && neighbor.agentNear != true)
                        {
                            //lo obtengo y remplazo el nodo anterior por el actual, y el actual por el nuevo
                            lastPatrolNode = actualPatrolNode;
                            return neighbor;
                        }
                    }
                }
            }
            else
            {   //sino, aplico recursion y llamo de nuevo al metodo
                return RandomNodeChange();
            }
        }
        //sino, por default devuelvo el vecino q este en el indice 0
        lastPatrolNode = actualPatrolNode;
        return actualPatrolNode.Neighbors[0];


    }
}
public class EggEscapeState
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
public class EggKidnapedState
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

