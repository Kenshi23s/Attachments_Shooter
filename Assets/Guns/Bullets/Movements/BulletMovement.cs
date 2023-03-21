using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BulletMovement
{

    public static BulletMovement MovementSelected(Transform tr,Rigidbody rb,BulletProperties bulletStats)
    {
        switch (bulletStats.movementType)
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
                return new BulletMovement_Default(tr, rb, bulletStats.stats[BulletProperties.Stat.Speed]);
             
        }
    }
    public enum Type
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

    protected BulletMovement(Transform _myTransform, Rigidbody _rb, float _speed)
    {
        this._myTransform = _myTransform;
        this._rb = _rb;
        this._speed = _speed;
    }
    public void Initialize()
    {
        _rb.velocity = Vector3.zero;
        


    }

    public abstract void MoveBullet();
}
