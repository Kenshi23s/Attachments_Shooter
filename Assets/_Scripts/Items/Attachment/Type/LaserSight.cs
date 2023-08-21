using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        GetComponent<PausableObject>().onPause += () => {if(isActiveAndEnabled) StartCoroutine(StopLaser()); };
        _laserLineRender.SetPosition(0, Vector3.zero);
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
   
  
    private void LateUpdate() => UpdateLaser();
    
    IEnumerator StopLaser()
    {
        _enable?.Invoke(false);
        UpdateLaser();
         yield return new WaitWhile(ScreenManager.IsPaused);
        _enable?.Invoke(isAttached);
    }

    void UpdateLaser()
    {
        if (Physics.Raycast(_laserOutPoint.position, _laserOutPoint.forward, out RaycastHit hit))        
            _laserLineRender.SetPosition(1, _laserLineRender.transform.InverseTransformPoint(hit.point));        
        else
            _laserLineRender.SetPosition(1, _laserLineRender.transform.InverseTransformPoint(_laserOutPoint.position + _laserOutPoint.forward * 200f));
    }
    

}
