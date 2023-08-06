using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatsHandler;
using static Attachment;
[RequireComponent(typeof(PausableObject))]
public class ShakeCamera : MonoBehaviour
{
    
    [Header("Caminar"), Space(1)]
    [SerializeField] float _walkFrequency = 14f;
    [SerializeField] float _walkAmplitude = 0.15f;
    [SerializeField] float _minWalkSpeed = 3f;

    [Header("Correr"), Space(1)]
    [SerializeField] float _runFrequency = 25f;
    [SerializeField] float _runAmplitude = 0.3f;
    [SerializeField] float _minRunSpeed = 12f;


    [Header("Suavizar Animacion"), Space(1)]
    [SerializeField, Range(0f, 1f)] float _soft = 0.5f;
    [SerializeField, Range(0f, 1f)] float _handsAmplitudeScale = 0.5f;
    [SerializeField, Range(0f, 1f)] float _horizontalFrequencyScale = 0.5f, _verticalFrequencyScale = 1f;

    [Header("Apuntado"), Space(1)]
    public Transform aimTransform;

    [SerializeField, Range(0, 1)]
    float _aimMultiplier = 0.1f;

    [SerializeField]
    float _aimFOV, _hipFOV;

    [SerializeField]
    float _aimSpeed = 8f, _returnSpeed = 8f;

    public static bool aiming = false;

    [Header("Referencias"), Space(1)]
    public Transform hands;
    public Camera cam;
    [SerializeField] Player_Movement playerMov;
    [SerializeField] Rigidbody rb;
    
    [SerializeField] GunHandler myGunHandler;

    public event Action OnCamUpdate;

    public static event Action<float> OnAimTransition;

    public static event Action OnHipPosReached, OnHipPosLeft, OnAimPosReached, OnAimPosLeft;
    bool _aimPosReached = false, _hipPosReached = false;

    Vector3 _camShake;
    Vector3 _handsShake;

    Vector3 _ogCamLocalPos;
    Vector3 _ogHandsLocalPos;
    Vector3 _currentHandsLocalPos;
    Vector3 _handsAimLocalPos;

    float _normalizedDistanceToAimPosition = 0;
    float _normalizedTargetPos;



    private void Awake()
    {
        _ogCamLocalPos = cam.transform.localPosition;
        _ogHandsLocalPos = hands.localPosition;
      
    }


    private void Start()
    {
        AimMotion();


    }

    public void Update()
    {
        if (ScreenManager.IsPaused()) return;

        ListenInputs();

        float handling = myGunHandler.ActualGun.stats.GetStat(StatNames.Handling);
        float multiplier = aiming ? _aimMultiplier : 1f;
        multiplier *= 1 - handling / 100f;
        //Debug.Log("handling: " + handling);

        SelectHandMovementType(multiplier);
        AimTransition();

        cam.transform.localPosition = _ogCamLocalPos + _camShake;
        hands.transform.localPosition = _currentHandsLocalPos + _handsShake;
    }



    void AimTransition()
    {
        // Si esta transicionando entre apuntar y hip fire...
        if (!HandsReachedTargetPosition())
        {
            // Sumar / Restar velocidad si esta apuntando
            _normalizedDistanceToAimPosition += aiming ? _aimSpeed * Time.deltaTime : -_returnSpeed * Time.deltaTime;
            _normalizedDistanceToAimPosition = Mathf.Clamp01(_normalizedDistanceToAimPosition);
            UpdateState();

            AimMotion();
        }
    }

    void SelectHandMovementType(float multiplier = 1)
    {
        // Movimiento de manos cuando corre
        if (rb.velocity.magnitude > _minRunSpeed && playerMov.onGrounded)
        {
            _camShake = _camShake * _soft + FootStepMotion(_runFrequency, _runAmplitude * multiplier);
            _handsShake = _camShake * _handsAmplitudeScale;
        }
        // Movimiento de manos cuando camina
        else if (rb.velocity.magnitude > _minWalkSpeed && playerMov.onGrounded)
        {

            _camShake = _camShake * _soft + FootStepMotion(_walkFrequency, _walkAmplitude * multiplier);
            _handsShake = _camShake * _handsAmplitudeScale;
        }
        // Movimiento de camera cuando esta quieto
        else
        {
            _camShake *= _soft;
            _handsShake *= _soft;
        }
    }

    void ListenInputs()
    {
        // Detectar inputs
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            AimInput();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            ReleaseAimInput();
        }
    }

    public void AimInput()
    {
        aiming = true;
        OnHipPosReached?.Invoke();
        // el hands aim local pos se deberia setear en un OnSightChange
        Vector3 offset = cam.transform.parent.InverseTransformPoint(myGunHandler.SightPosition);
        offset.y -= _handsShake.y;
        offset.x -= _handsShake.x;
        offset.z = 0;
        _handsAimLocalPos = _currentHandsLocalPos - offset;

        _normalizedTargetPos = 1;
    }

    void ReleaseAimInput()
    {
        OnAimPosReached?.Invoke();
        aiming = false;
        _normalizedTargetPos = 0;
    }

    void AimMotion()
    {
        float smoothT = Mathf.SmoothStep(0, 1, _normalizedDistanceToAimPosition);
        _currentHandsLocalPos = Vector3.Lerp(_ogHandsLocalPos, _handsAimLocalPos, smoothT);
        LerpCameraFOV(smoothT);
        OnAimTransition?.Invoke(smoothT);
    }

    bool HandsReachedTargetPosition() 
    {
        return _normalizedTargetPos - _normalizedDistanceToAimPosition == 0;
    }

    void UpdateState() 
    {
        // Llamar eventos y actualizar variables
        if (_aimPosReached)
        {
            _aimPosReached = _normalizedDistanceToAimPosition == 1;
            if (!_aimPosReached) 
            {
                OnAimPosLeft?.Invoke();
                //Debug.Log("AIM POS LEFT");

            }
        }
        else
        {
            _aimPosReached = _normalizedDistanceToAimPosition == 1;
            if (_aimPosReached) 
            {
                OnAimPosReached?.Invoke();
                //Debug.Log("AIM POS REACHED");
            }
        }

        if (_hipPosReached)
        {
            _hipPosReached = _normalizedDistanceToAimPosition == 0;
            if (!_hipPosReached) 
            {
                OnHipPosLeft?.Invoke();
                //Debug.Log("HIP POS LEFT");
            }
        }
        else
        {
            _hipPosReached = _normalizedDistanceToAimPosition == 0;
            if (_hipPosReached) 
            {
                OnHipPosReached?.Invoke();
                //Debug.Log("HIP POS REACHED");
            }
        }
    }

    void LerpCameraFOV(float t)
    {
        float multiplyZoom = 1;
      
        if (myGunHandler.ActualGun.attachmentHandler.TryGetAttachment<Sight>(AttachmentType.Sight,out var sight))        
            multiplyZoom = sight.zoomMultiplier;
        
        cam.fieldOfView = Mathf.Lerp(_hipFOV, _aimFOV/Mathf.Max(1,multiplyZoom), t);
    }

    public Vector3 FootStepMotion(float frequency, float amplitude)
    {
        Vector3 pos = Vector3.zero;
        pos.x += Mathf.Cos(Time.time * frequency * _horizontalFrequencyScale) * amplitude;
        pos.y += Mathf.Sin(Time.time * frequency * _verticalFrequencyScale) * amplitude;
        return pos;
    }

    private void OnValidate()
    {
        _hipFOV = cam.fieldOfView;
    }
}
