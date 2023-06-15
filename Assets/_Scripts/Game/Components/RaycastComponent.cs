using System;
using UnityEngine;
using FacundoColomboMethods;
using static StatsHandler;
using System.Collections;

[RequireComponent(typeof(DebugableObject))]
public class RaycastComponent : MonoBehaviour
{
   [SerializeField] TrailRenderer trailSample;
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

        Vector3 dir = cam.transform.forward;
        if (!Input.GetKey(KeyCode.Mouse1))
        {
            int multiplier = 45;
            float aux = multiplier - gun.stats.GetStat(StatNames.Spread) * multiplier / 100;
            gun._debug.Log("mouse apretado");
            dir = cam.transform.forward.RandomDirFrom(aux); 
        }

        var z = Instantiate(trailSample, from, Quaternion.identity);
        Vector3 dirTrail=Vector3.zero;
        if (Physics.Raycast(cam.transform.position, dir, out RaycastHit testHit, 500f, _shootableLayers))
        {
            gun._debug.Log("RAYCAST FROM CAM HIT: " + testHit.transform.gameObject);

            Vector3 dir2 = testHit.point - from;

            // Disparar raycast desde el arma hacia la posicion que golpeo el raycast de la camara
            if (Physics.Raycast(from, dir2, out RaycastHit actualhit, dir2.magnitude + 1, _shootableLayers))
            {

                dirTrail = actualhit.point;


                CheckDamagable(actualhit, action);
                  

                if (actualhit.transform.TryGetComponent(out IHitFeedback x))
                    x.FeedbackHit(actualhit.point, actualhit.normal);

                gun._debug.Log("RAYCAST FROM GUN HIT: " + actualhit.transform.gameObject);
             
            }


        }
        if (dirTrail==Vector3.zero) dirTrail = cam.transform.forward * 100;

        StartCoroutine(SpawnTrail(z, dirTrail));
      

    }


    IEnumerator SpawnTrail(TrailRenderer trail,Vector3 impactPos)
    {
        Vector3 startPos = trail.transform.position;
        float dist = Vector3.Distance(startPos, impactPos);
        float time = 0f;
  
        while (time < 1) 
        {
           
            trail.transform.position = Vector3.Lerp(startPos, impactPos, time);

           
            time += (Time.deltaTime / trail.time) * (dist / Vector3.Distance(trail.transform.position,impactPos));
            yield return null;
        }
        trail.transform.position = impactPos;
        Destroy(trail.gameObject);
    }
    //chequea si lo que con lo que choco es damagable
    void CheckDamagable(RaycastHit hit,Action<HitData> action)
    {
        
        if (hit.transform.TryGetComponent(out IDamagable damagable))
        {
            HitData hitData = new HitData();
            // 100 es el maximo de rango.
            float rangeMultiplier =  1 - hit.distance / gun.stats.GetStat(StatNames.Range);
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
            action?.Invoke(hitData);
        }
       
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
