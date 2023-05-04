using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Turret : Enemy
{
    [Header("Turret")]
    [SerializeField] float _detectionRadius;
    [SerializeField] int _bulletDamage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField, Range(0, 180)] float _spreadAngle;

    [SerializeField] Transform _canon;
    [SerializeField] Transform[] _shootPos;
    [SerializeField] int _actualShootPos;

    [SerializeField] GameObject bulletPrefab;






    bool AlignCanon(Vector3 playerPos)
    {
        Vector3 desired_dir = playerPos - transform.position;
        _canon.transform.forward = Vector3.Slerp(_canon.transform.forward, desired_dir.normalized, _rotationSpeed * Time.deltaTime);

        return Vector3.Distance(desired_dir, _canon.transform.forward) < 1;

    }


    void AlignCanonAndShoot(Vector3 playerPos)
    {
        AlignCanon(playerPos);

        RaycastHit hit;
        Vector3 dir = _shootPos[_actualShootPos].forward.RandomDirFrom(_spreadAngle);
        if (Physics.Raycast(_shootPos[_actualShootPos].position, dir, out hit))
        {
            if (hit.transform.TryGetComponent(out Player_Movement player))
            {
                player.lifeHandler.TakeDamage(_bulletDamage);
            }
        }
    }

    void ChangeShootPos() => _actualShootPos = _actualShootPos >= _shootPos.Length ? 0 : _actualShootPos++;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 dir = _shootPos[_actualShootPos].forward.RandomDirFrom(_spreadAngle) * 10;
        Gizmos.DrawLine(_shootPos[_actualShootPos].position, transform.position + dir);
        ChangeShootPos();
    }
}
