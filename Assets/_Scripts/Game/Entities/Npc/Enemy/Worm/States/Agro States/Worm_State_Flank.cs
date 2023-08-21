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
        for (int i = 0; i < flankPositions.Length; i++)
            flankPositions[i].Normalize();

    }

    Vector3[] flankPositions = {
        new Vector3(-1, 0, -1),
        new Vector3( 0, 0, -1),
        new Vector3( 1, 0, -1)
    };

    Vector3 _flankPos;
    public override void OnEnter()
    {
        // Conseguir posicion de flankeo al azar
        Vector3 randomPos = flankPositions[Random.Range(0, flankPositions.Length)];

        // Conseguir posicion en espacio de mundo
        Vector3 dirToPlayer = Player_Handler.position - _worm.transform.position;
        dirToPlayer.y = 0;
        Vector3 flankPosDirFromPlayer = Quaternion.FromToRotation(Vector3.forward, randomPos) * dirToPlayer.normalized; 
        Vector3 flankPosition = Player_Handler.position + flankPosDirFromPlayer * _worm.FlankDistance;

        // Conseguir direccion a esa posicion
        //Vector3 flankDir = flankPosition - _worm.transform.position;

        // Chequear si hay alguna pared obstruyendo el camino
        //if (Physics.Raycast(_worm.transform.position + Vector3.up, flankDir, out RaycastHit hit, flankDir.magnitude + 0.5f, IA_Manager.instance.wall_Mask))
        //{
        //    flankPosition = _worm.transform.position + flankDir.normalized * (hit.distance - 0.5f);           
        //}

        _worm.AI_move.OnDestinationReached += FlankComplete;
        _worm.AI_move.SetDestinationButLookAt(flankPosition, Player_Handler.Transform);
        _flankPos = _worm.AI_move.Destination;
    }

    void FlankComplete() {
        _worm.fsm.ChangeState(EWormStates.Attack);
    }

    public override void OnExit()
    {
        _worm.AI_move.OnDestinationReached -= FlankComplete;
        _worm.anim.SetTrigger("StopFlanking");
    }

    public override void GizmoShow()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_flankPos, 0.5f);
    }
}
