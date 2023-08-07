using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PausableObject))]
public class LaserSight : Attachment
{
    [SerializeField] LineRenderer _laserLineRender;
    [SerializeField] Transform _laserOutPoint;
    Action<bool> _enable;

    protected override void Initialize()
    {
        MyType = AttachmentType.LaserSight;
        GetComponent<PausableObject>().onPause += () => {if(this.isActiveAndEnabled) StartCoroutine(StopLaser()); };
    }

    private void OnValidate()
    {
        UpdateLaser(_laserOutPoint.forward);
    }

    protected override void Comunicate() 
    {
        _enable = (x) =>
        {
            _laserLineRender.gameObject.SetActive(x);
            enabled = x;
        };

        OnAttach += () => _enable(!ScreenManager.IsPaused());
        OnDettach += () => _enable(false);     
    }
   
  
    private void Update() => UpdateLaser(Camera.main.transform.forward);
    
    IEnumerator StopLaser()
    {
        _enable?.Invoke(false);
        UpdateLaser(_laserOutPoint.forward);
         yield return new WaitWhile(ScreenManager.IsPaused);
        _enable?.Invoke(isAttached);
    }

    void UpdateLaser(Vector3 forward)
    {

        
        _laserLineRender.SetPosition(0, _laserOutPoint.position);
        if (Physics.Raycast(_laserOutPoint.position, forward, out RaycastHit hit))        
            _laserLineRender.SetPosition(1, hit.point);        
        else
            _laserLineRender.SetPosition(1, _laserOutPoint.position + forward * 200f);
    }
    

}
