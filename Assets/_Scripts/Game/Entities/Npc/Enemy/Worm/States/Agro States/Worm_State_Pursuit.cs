using static Worm_State_Attack;

public class Worm_State_Pursuit : Worm_State<Worm_AttackState>
{

    public Worm_State_Pursuit(Enemy_Worm worm) : base(worm)
    {
    }

    public override void OnEnter()
    {
        _worm.anim.SetBool("Pursuit", true);
        _worm.AI_move.Movement.RemoveForces();
    }

    public override void OnUpdate()
    {
        // NOTA: Esto se podria optimizar si se sabe que rango es mas chico. En ese caso primero se chequearia por 
        // el rango mas chico, y recien despues por los mas grandes.
        bool inAcidRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.ShootAcidRadius);
        bool inDirtRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.ShootDirtRadius);
        bool inMeleeRange = _worm.AI_move.FOV.IN_FOV(Player_Movement.position, _worm.MeleeAttackRadius);

        // MELEE
        if (inMeleeRange)
        {
            _fsm.ChangeState(Worm_AttackState.Melee);
            return;
        }

        // Deberiamos tener una frecuencia/ratio para las chances de que el gusano haga uno u otro ataque.
        // Si esta lo suficientemente cerca, hace melee
        // RANGED
        if (inDirtRange)
        {
            _fsm.ChangeState(Worm_AttackState.GrabDirt);
            return;
        }

        if (inAcidRange)
        {
            _fsm.ChangeState(Worm_AttackState.ShootAcid);
            return;
        }


        _worm.AI_move.SetDestination(Player_Movement.position);
    }

    public override void OnExit()
    {
        _worm.AI_move.CancelMovement();
        _worm.anim.SetBool("Pursuit", true);
    }
}
