using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AirTurret : MonoBehaviour
{
    Transform pivotBaseRotation, pivotMisileRotation;
    float _rotationX;


    Action align;
    public void AlignCanon(Transform _target)
    {
        Vector3 dir = _target.position - transform.position;
        Vector3 desiredForward = new Vector3(0,0, dir.z) - transform.forward;
        pivotBaseRotation.forward += desiredForward.normalized * Time.deltaTime * _rotationX;
        //if (Vector3.Distance(pivotBaseRotation.forward>))
        //{

        //}
    }
    
}
