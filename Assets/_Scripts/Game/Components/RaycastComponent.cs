using System;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class RaycastComponent : MonoBehaviour
{
    Gun gun;
    [SerializeField]
    LayerMask _shootableLayers;
    [SerializeField]
    Camera cam;
    // se encarga de manejar los disparos con raycast del arma
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
        // Primero disparar raycast desde la camara
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit testHit, 500f, _shootableLayers))
        {
            gun._debug.Log("RAYCAST FROM CAM HIT: " + testHit.transform.gameObject);

            Vector3 dir = testHit.point - from;

            // Disparar raycast desde el arma hacia la posicion que golpeo el raycast de la camara
            if (Physics.Raycast(from, dir, out RaycastHit actualhit, dir.magnitude + 1, _shootableLayers))
            {
                if (CheckDamagable(actualhit, out HitData hitData))
                    action?.Invoke(hitData);

                if (actualhit.transform.TryGetComponent(out IHitFeedback x))
                    x.FeedbackHit(actualhit.point, actualhit.normal);

                gun._debug.Log("RAYCAST FROM GUN HIT: " + actualhit.transform.gameObject);
            }


        }
    }
    //chequea si lo que con lo que choco es damagable
    bool CheckDamagable(RaycastHit hit, out HitData hitData)
    {
        hitData = default;
        if (hit.transform.TryGetComponent(out IDamagable damagable))
        {
            // 100 es el maximo de rango.
            float rangeMultiplier =  1 - hit.distance / gun.stats.GetStat(StatsHandler.StatNames.Range);
            #region coment
            //gun._debug.Log("Range multiplier: " + rangeMultiplier);
            //gun._debug.Log("CURRENT DAMAGE: " + gun.damageHandler.currentDamage);
            //gun._debug.Log("BEFORE CAST: " + gun.damageHandler.currentDamage * rangeMultiplier);
            //gun._debug.Log("AFTER CAST: " + (int) (gun.damageHandler.currentDamage * rangeMultiplier));
            #endregion
            int finalDamage = Mathf.Max(1, (int) (gun.damageHandler.currentDamage * rangeMultiplier));
            gun._debug.Log("Final damage: " + finalDamage);


            hitData.dmgData = damagable.TakeDamage(finalDamage, hit.point);
            hitData.dmgData.victim = damagable;
            hitData._impactPos = hit.point;
            hitData.gunUsed = gun;
           
        }
        return !hitData.Equals(default);
    }

    void DrawRaycast()
    {
        if (gun.attachmentHandler._shootPos == null||cam==null) return;
       
        Gizmos.color = Color.blue;
        Vector3 from = gun.attachmentHandler._shootPos.position;

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
