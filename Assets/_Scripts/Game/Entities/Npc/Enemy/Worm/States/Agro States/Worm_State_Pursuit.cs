public class Worm_State_Pursuit : Worm_State
{
    public Worm_State_Pursuit(Enemy_Worm worm) : base(worm)
    {
    }

    public override void OnEnter()
    {
        _worm.anim.SetBool("Pursuit", true);
    }

    public override void OnUpdate()
    {
        // Chequear que el enemigo este en dentro del rango de ataque, y si lo esta, pasar a
        // disparo de acido/
        // carga de tierra/
        // melee
        // Deberiamos tener una frecuencia/ratio para las chances de que el gusano haga uno u otro ataque.
        // Si esta lo suficientemente cerca, 
        //


        // Hacer que, si el jugador esta visible, lo mire. Y si no que mire a la direccion en la que avanza y lo persiga al 
        //_worm.AI_move.SetDestination(Player_Movement.position);


    }

    public override void OnExit()
    {
        //_worm.anim.SetBool("Pursuit", true);
    }
}
