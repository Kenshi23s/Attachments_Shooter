using FacundoColomboMethods;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Enemy_Worm;
using static Worm_State_Attack;

public class Worm_State_Flank : Worm_State<Worm_AttackState>
{
    public Worm_State_Flank(Enemy_Worm worm) : base(worm) 
    {
        onFlankComplete = () => _worm.fsm.ChangeState(EWormStates.Attack);

        for (int i = 0; i < flankDirections.Length; i++)
            flankDirections[i].Normalize();

    }

    Action onFlankComplete;
    float unitsAway = 2;
    Vector3 pointToFlank = Vector3.zero;

    Vector3[] flankDirections = {
        new Vector3(-1, 0,  0),
        new Vector3(-1, 0, -1),
        new Vector3( 0, 0, -1),
        new Vector3( 1, 0, -1),
        new Vector3( 1, 0, 0)};

    public override void OnEnter()
    {
      
        Vector3 randomDir = flankDirections[Random.Range(0, flankDirections.Length)];
        Vector3 dir = Player_Movement.position - _worm.transform.position;
        Vector3 dirToFlank = Quaternion.FromToRotation(Vector3.forward, randomDir) * dir.normalized;

        int layer = IA_Manager.instance.wall_Mask.LayerBitmaskToInt();

        if (Physics.Raycast(_worm.transform.position + Vector3.up, dirToFlank, out RaycastHit hit, unitsAway, layer))
        {
            pointToFlank = hit.point - dirToFlank * 0.65f;           
        }                
        else        
            pointToFlank = _worm.transform.position + dirToFlank.normalized * unitsAway;

        _worm.AI_move.OnDestinationReached += onFlankComplete;
        _worm.AI_move.SetDestination(pointToFlank);
    }
    
    

    public override void OnExit()
    {
        _worm.anim.SetTrigger("StopFlanking");
        _worm.AI_move.OnDestinationReached -= OnExit;
    }


    public override void GizmoShow()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pointToFlank,2f);
        Gizmos.DrawLine(_worm.transform.position, pointToFlank);
    }
}
