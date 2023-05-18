using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class RaycastComponent : MonoBehaviour
{
    Gun gun;
    [SerializeField]
    LayerMask _shootableLayers;
    [SerializeField]
    Camera cam;
    private void Start()
    {
        gun = GetComponent<Gun>();
        if (cam == null)
        {
            cam = Camera.main;
        }
        gun._debug.AddGizmoAction(DrawRaycast);
    }

    public void ShootRaycast(Vector3 from, Action<HitData> action)
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit testHit, 500f, _shootableLayers))
        {
            Vector3 dir = testHit.point - from;
            if (Physics.Raycast(from, dir, out RaycastHit actualhit, dir.magnitude, _shootableLayers))
            {
                if (CheckDamagable(actualhit, out HitData hitData))
                    action?.Invoke(hitData);

                if (actualhit.transform.TryGetComponent(out IHitFeedback x))
                    x.FeedbackHit(actualhit.point, actualhit.normal);
            }
        }
    }

    bool CheckDamagable(RaycastHit hit, out HitData hitData)
    {
        hitData = default;
        if (hit.transform.TryGetComponent(out IDamagable damagable))
        {
            hitData._impactPos = hit.point;
            hitData.gunUsed = gun;
            hitData.dmgData = damagable.TakeDamage(gun.damageHandler.actualDamage);
        }
        return !hitData.Equals(default);
    }

    void DrawRaycast()
    {
        if (gun.attachmentHandler.shootPos == null||cam==null) return;
       
        Gizmos.color = Color.blue;
        Vector3 from = gun.attachmentHandler.shootPos.position;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit testHit, 500f, _shootableLayers))
        {
            Vector3 dir = testHit.point - from;
            if (Physics.Raycast(from, dir, out RaycastHit actualhit, dir.magnitude, _shootableLayers))
            {
               Gizmos.DrawLine(from, dir);
               Gizmos.DrawWireSphere(actualhit.point, 5);
                return;
            }
        }
        Gizmos.DrawLine(from, from+ cam.transform.forward * 30f);
    }




}
