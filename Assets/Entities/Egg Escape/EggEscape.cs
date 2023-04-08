using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EggEscapeModel;


[RequireComponent(typeof(NavMeshAgent))]
public class EggEscapeModel : Entity
{  
   
    
    public struct EggStates_Var
    {
        public NavMeshAgent agent;
        public StateMachine<EggStates> _fsm;
        public EggGameChaseMode gameMode;
        public FOVAgent fov;
    }
    EggStates_Var _eggStates_Var;

    public enum EggStates
    {
        Patrol,
        Escape,
        Kidnapped
    }
    //tendria que tener 3 estados:
    // Patrullar, hago la mia y sigo los waypoints, trato de no hacer el mismo camino q otro huevo en caso q haya(Obsoleto)
    // Escapar: vi al player con FOV al patrullar asi q me re tomo el palo en la direccion contraria a la q viene
    // Liberarse: el player me agarro, desp de x segundos me libero y paso directo a el estado escapar
   

    public void Initialize(EggGameChaseMode gameMode,Vector3 SpawnPos)
    {
        transform.position = SpawnPos;

        _eggStates_Var = new EggStates_Var();

        #region Setting Struct
        _eggStates_Var.agent = GetComponent<NavMeshAgent>();
        _eggStates_Var.gameMode= gameMode;
        _eggStates_Var.fov = new FOVAgent(transform);
        _eggStates_Var._fsm = new StateMachine<EggStates>();
        #endregion

        #region Setting Finite State Machine
        _eggStates_Var._fsm.CreateState(EggStates.Patrol,new EggPatrolState(_eggStates_Var));
        _eggStates_Var._fsm.CreateState(EggStates.Escape, new EggEscapeState(_eggStates_Var));
        #endregion

        _eggStates_Var._fsm.ChangeState(EggStates.Patrol);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() => _eggStates_Var._fsm.Execute();

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    public override void Resume()
    {
        throw new System.NotImplementedException();
    }

    public override int TakeDamage(int dmgValue)
    {
        int _dmgDealt = lifeHandler.Damage(dmgValue);
        if (lifeHandler.life<=0)
        {
            _eggStates_Var._fsm.ChangeState(EggStates.Escape);
        }

        return _dmgDealt;
    }

    public void EscapeSuccesfull()
    {
        lifeHandler.Heal(lifeHandler.maxLife);
        _eggStates_Var._fsm.ChangeState(EggStates.Escape);
    }

    public override bool WasCrit() => false;
   

    public override bool WasKilled()=> false;
}
