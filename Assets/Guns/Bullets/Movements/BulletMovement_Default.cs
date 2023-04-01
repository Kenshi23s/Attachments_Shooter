using UnityEngine;

public class BulletMovement_Default : BulletMovement
{
    public BulletMovement_Default(Transform _myTransform, Rigidbody _rb) : base(_myTransform, _rb)
    {
    }

    public override void MoveBullet()
    {
        float t = Time.deltaTime * speed * ScreenManager.time;
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + _myTransform.forward * t ,speed);
      
    }

 
}
