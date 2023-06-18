using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatsHandler;
[RequireComponent(typeof(PausableObject))]
public class ShakeCamera : MonoBehaviour
{
    
    [Header("Caminar"), Space(1)]
    public float _walkFrequency = 0.7f;
    public float _walkAmplitude = 1f;
    public float _minWalk = 1f;

    [Header("Correr"), Space(1)]
    public float _runFrequency = 1f;
    public float _runAmplitude = 1f;
    public float _minRun = 1f;

    [Header("Suavizar Animacion"), Space(1)]
    [Range(0f, 1f)]
    public float soft = 0.3f;
    [Range(0f, 1f)]
    public float handsInfluence = 0.5f;

    [Header("Apuntado"), Space(1)]
    public Transform aimTransform;
   
    public static bool aiming =false;
    Vector3 actualAimPos;

    [Header("Referencias"), Space(1)]
    public Transform hands;
    public Camera cam;
    [SerializeField] Player_Movement playerMov;
    [SerializeField] Rigidbody rb;
    
    [SerializeField] GunHandler myGunHandler;

    public event Action OnCamUpdate;


    [SerializeField]
    float aimFov, hipFov,Speed,returnSpeed;
    private void Awake()
    {
      
       
    }

    public void Update()
    {
        if (ScreenManager.IsPaused()) return;

        Vector3 camShake;
        Vector3 handsShake;
        // Movimiento de manos cuando corre
        if (rb.velocity.magnitude > _minRun && playerMov.onGrounded)
        {
            camShake = Vector3.Lerp(cam.transform.localPosition, FootStepMotion(_runFrequency, _runAmplitude), soft);
            handsShake = Vector3.Lerp(hands.localPosition, FootStepMotion(_runFrequency, _runAmplitude) * handsInfluence, soft);
        }
        // Movimiento de manos cuando camina
        else
        {
            if (rb.velocity.magnitude > _minWalk && playerMov.onGrounded)
            {
                camShake = Vector3.Lerp(cam.transform.localPosition, FootStepMotion(_walkFrequency, _walkAmplitude), soft);
                handsShake = Vector3.Lerp(hands.localPosition, FootStepMotion(_walkFrequency, _walkAmplitude) * handsInfluence, soft);
            }
            // Movimiento de camera cuando esta quieto
            else
            {
                camShake = Vector3.Lerp(cam.transform.localPosition, Vector3.zero, soft);
                handsShake = Vector3.Lerp(hands.localPosition, Vector3.zero, soft);
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            AimMotion();
            float handling = myGunHandler.actualGun.stats.GetStat(StatNames.Handling);
            LerpCamera(aimFov, soft + (soft * handling / 100)*Time.deltaTime, aimFov >= cam.fieldOfView);


        } 
        else
        {
            LerpCamera(hipFov, returnSpeed*Time.deltaTime, hipFov <= cam.fieldOfView);
            aiming = false;   
            hands.transform.localPosition = handsShake;
            actualAimPos = Vector3.zero;
        }
        cam.transform.localPosition = camShake;

       
    }

    void AimMotion()
    {
        Vector3 targetAimPos = -cam.transform.InverseTransformPoint(myGunHandler.SightPosition);
        targetAimPos.z = 0;
        aiming = true;
        float handling = myGunHandler.actualGun.stats.GetStat(StatNames.Handling);
        actualAimPos = Vector3.Lerp(actualAimPos, hands.transform.localPosition + targetAimPos, soft + (soft * handling / 100) * Time.deltaTime);
        hands.transform.localPosition = actualAimPos;
        
    }

    void LerpCamera(float lerpTo,float lerpSpeed,bool conditionToStop)
    {
        if (!conditionToStop)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, lerpTo, lerpSpeed);
        else
            cam.fieldOfView = lerpTo;
    }
     
  

    public Vector3 FootStepMotion(float frequency, float amplitude)
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude*1.2f;
        pos.x += Mathf.Cos(Time.time * frequency/2) * amplitude * 2;
        return pos;
    }
    private void OnValidate()
    {
        hipFov = cam.fieldOfView;
    }
}
