using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BulletMovement
{
    // se debe seleccionar el movimiento SOLO EN EL AWAKE(es decir, al inicializar)
    public static BulletMovement MovementSelected(Transform tr,Rigidbody rb, BulletMoveType type )
    {
        switch (type)
        {
            
            //case Type.Tracking:
            //    break;
            //case Type.Accelerating:
            //    break;
            //case Type.Misile:
            //    break;
            //case Type.FallOff:
            //    break;
            default:
                return new BulletMovement_Default(tr, rb);
             
        }
    }
    public enum BulletMoveType
    {
        Default,
        Tracking,
        Accelerating,
        Misile,
        FallOff

    }
    protected Rigidbody _rb;
    [SerializeField]float _speed;
    public float speed => _speed;
    protected Transform _myTransform;

    protected BulletMovement(Transform _myTransform, Rigidbody _rb)
    {
        this._myTransform = _myTransform;
        this._rb = _rb;
    }
    public virtual void Initialize()
    {
        _rb.velocity = Vector3.zero;
        Vector3 aux = Player_Movement._velocity != Vector3.zero ? Player_Movement._velocity : Vector3.one;

        _rb.AddForce(aux + (_rb.transform.forward* speed * 10) ,ForceMode.Force);
    }
    public void SetSpeed(float _speed)
    {
        this._speed = _speed;
        Debug.Log("speed Change");
    } 
 
    public abstract void MoveBullet();
}
