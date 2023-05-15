using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : Attachment
{
    [SerializeField] LineRenderer _laserLineRender;
    [SerializeField] Transform _laserOutPoint;


    protected override void Initialize()
    {
     

        _myType = AttachmentType.LaserSight;
        _laserLineRender = GetComponentInChildren<LineRenderer>();

        Action<bool> enable = (x) =>
        {
            _laserLineRender.gameObject.SetActive(x);
            enabled = x;
        };

        onAttach += () => enable(true);
        onDettach += () => enable(false);
        enable(isAttached);
    }

    private void Update() => UpdateLaser();
    

    void UpdateLaser()
    {
        Vector3 forward = Camera.main.transform.forward;
        _laserLineRender.SetPosition(0, _laserOutPoint.position);
        if (Physics.Raycast(_laserOutPoint.position, forward, out RaycastHit hit))        
            _laserLineRender.SetPosition(1, hit.point);        
        else
            _laserLineRender.SetPosition(1, _laserOutPoint.position + forward * 200f);
    }

}
