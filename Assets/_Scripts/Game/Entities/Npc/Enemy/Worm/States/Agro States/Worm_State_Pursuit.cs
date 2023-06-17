using UnityEngine;
using static Worm_State_Attack;

public class Worm_State_Pursuit : Worm_State<Worm_AttackState>
{

    public Worm_State_Pursuit(Enemy_Worm worm) : base(worm)
    {
    }

    public override void OnEnter()
    {
        _worm.anim.SetBool("Moving", true);
        _worm.AI_move.Movement.RemoveForces();
    }

    public override void OnUpdate()
    {
        // NOTA: Esto se podria optimizar si se sabe que rango es mas chico. En ese caso primero se chequearia por 
        // el rango mas chico, y recien despues por los mas grandes.
        bool inAcidRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.ShootAcidRadius);
        bool inDirtRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.ShootDirtRadius);
        bool inMeleeRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.MeleeAttackRadius);

         Worm_AttackState key =
           _worm.CanMelee && inMeleeRange ? Worm_AttackState.Melee
         : inDirtRange ? Worm_AttackState.GrabDirt
         : inAcidRange ? Worm_AttackState.ShootAcid
         : default;

        if (key != default)
        {
            _fsm.ChangeState(key);
            Vector3 dir = Player_Movement.position-_worm.transform.position;
            if (Physics.Raycast(_worm.transform.position, dir,out RaycastHit hit))
            {
                
                _worm.AssignTarget(hit.transform);
            }
           
        }
           
        else 
            _worm.AI_move.SetDestination(Player_Movement.position);
    }

    public override void OnExit()
    {
        _worm.anim.SetBool("Moving", false);
        _worm.AI_move.CancelMovement();
    }
}
