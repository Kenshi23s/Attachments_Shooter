using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerMovement : IPlayerState
{
    Rigidbody _rb;
    float _speed;
    float _maxVelocity;
    float _friction;
    float _lurch;
    float _lurchAngle;
    float _groundDistance;
    CapsuleCollider _mycolision;
    LayerMask _mycollider;
    bool _onGrounded;

    #region mis metodos de state
    public virtual void OnEnter()
    {

    }

    public virtual void OnVirtualUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
    #endregion

    #region herramientas de movimiento
    public void LimitVelocity()
    {
        Vector3 HorizontalVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

        if (HorizontalVelocity.magnitude > _maxVelocity)
        {
            HorizontalVelocity = HorizontalVelocity.normalized * _maxVelocity;
            _rb.velocity = new Vector3(HorizontalVelocity.x, _rb.velocity.y, HorizontalVelocity.z);
        }
    }

    public void Friction()
    {
        Vector3 tempDir = _rb.velocity.normalized;
        _rb.velocity -= _rb.velocity.normalized * _friction * Time.deltaTime;

        if (_rb.velocity.normalized * -1 == tempDir)
        {
            _rb.velocity = Vector3.zero;
        }
    }

    public void SetVelocity(Vector3 dir, float force)
    {
        _rb.velocity = dir.normalized * force;
    }

    public void DetectOnGrounded(LayerMask myCollider, CapsuleCollider _myCol)
    {
        RaycastHit myhit;

        _onGrounded = (Physics.SphereCast(_rb.position + (Vector3.down * ((_myCol.height / 2) - _myCol.radius * 2) * _rb.transform.localScale.y),
                                    _myCol.radius * _rb.transform.localScale.y, -_rb.transform.up, out myhit, _groundDistance, myCollider)
                               && !myhit.collider.isTrigger);
    }

    //falta desarrollar
    public void Lurch()
    {

    }

    #endregion
}
