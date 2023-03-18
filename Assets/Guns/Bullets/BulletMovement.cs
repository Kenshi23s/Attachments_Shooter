using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Default,
    Tracking,
    Accelerating,
    Misile,
    FallOff

}
public abstract class BulletMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]float speed;

    public BulletMovement(Rigidbody rb, float speed)
    {
        this.rb = rb;
        this.speed = speed;
    }

    public abstract void MoveBullet();
}
