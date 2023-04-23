using UnityEngine;

public class StatePlayerMovement
{
    Rigidbody _rb;
    float _maxVelocity;
    float _friction;

    public void Initialize(Rigidbody rb, float maxvelocity, float friction)
    {
        _rb = rb;
        _maxVelocity = maxvelocity;
        _friction = friction;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnVirtualUpdate()
    {

    }

    public virtual void OnExit()
    {

    }

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

    //falta desarrollar
    public void Lurch()
    {

    }

    #endregion
}
