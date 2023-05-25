using UnityEngine;

public class BulletMovement_Default : BulletMovement
{

    public override void Initialize() { }


    private void FixedUpdate()
    {
        Vector3 playerVelocity = Player_Movement.velocity != Vector3.zero 
            ? Player_Movement.velocity 
            : Vector3.one;

        Vector3 force = transform.forward * _movement.maxForce;

        _movement.AddForce(playerVelocity + force*10);
    }
  

 
}
