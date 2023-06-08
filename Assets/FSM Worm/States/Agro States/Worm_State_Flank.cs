using FacundoColomboMethods;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Enemy_Worm;
public class Worm_State_Flank : Worm_State
{
    public Worm_State_Flank(Enemy_Worm worm) : base(worm) 
    {
        onFlankComplete = () => _worm.fsm.ChangeState(EWormStates.Attack);
    }
    Action onFlankComplete;
    float unitsAway;
    Vector3 pointToFlank = Vector3.zero;

    public override void OnEnter()
    {
      
        Vector3 randomDir = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 1));
        Vector3 dir = Player_Movement.position - _worm.transform.position;
        Vector3 dirToFlank = dir.normalized + randomDir.normalized;

       
        int layer = IA_Manager.instance.wall_Mask.LayerBitmaskToInt();

        if (Physics.Raycast(_worm.transform.position, dirToFlank, out RaycastHit hit, unitsAway, layer))
        {
            pointToFlank = hit.point;
           
        }                
        else        
            pointToFlank = dirToFlank.normalized * unitsAway;

        _worm.AI_move.OnDestinationReached += onFlankComplete;
        _worm.AI_move.SetDestination(pointToFlank);
    }
    
    

    public override void OnExit()
    {
        _worm.AI_move.OnDestinationReached -= OnExit;
    }


    public override void GizmoShow()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pointToFlank,2f);
        Gizmos.DrawLine(_worm.transform.position, pointToFlank);
    }
}
