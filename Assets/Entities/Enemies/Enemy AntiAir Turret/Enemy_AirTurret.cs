using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy_AirTurret : MonoBehaviour
{
    //aca me guardo el target actual
    public Transform target => _target;
    Transform _target;
    StateMachine<string> _fsm;

    #region Pivots
    //x                   //y //pensar mejores nombres
    [Header("Base Pivot"), Space]
    [SerializeField] Transform pivotBase;
    [SerializeField] float _baseRotationSpeed;

    [Header("Misile Battery Pivot"), Space]
    [SerializeField] Transform pivotMisileBattery;
    [SerializeField, Range(0, 90f)] float _maxBatteryAngle;
    [SerializeField] float _canonRotationSpeed;
 

    [Space,SerializeField] Transform[] _shootPositions;
    #endregion

    #region Misile
    [Header("Misile"), Space(10f)]
   
    [SerializeField]TurretMisile.MisileStats _misileStats;
 
    [SerializeField, Space] TurretMisile misilePrefab;
    [SerializeField] float misilesPerShoot;
    #endregion

    #region Cooldowns
    [Header("Cooldowns"), Space]
    [SerializeField,Tooltip("El tiempo entre misil y misil en una rafaga de misiles")] 
    float cd_BetweenMisiles;
    [SerializeField,Tooltip("El tiempo entre rafaga de misiles y rafaga de misiles")]  
    float cd_Volleys;
    #endregion
   
    #region Solo Lectura en editor
    //desp hacerle una pool
    [Header("VARIABLES DE SOLO LECTURA"), Space]
    [SerializeField]float actualMisileCD;
    [SerializeField]float actualVolleyCD;
    [SerializeField]float misilesLeft;
    #endregion


    Action TurretUpdate;

    private void Update()
    {
        TurretUpdate?.Invoke();
        _fsm.Execute();
    }
    //tendria q hacer otra clase q haga un callback hacia esta?
    void TargetDetected(Transform target)
    {
        if (target!=null)
        {
            this._target = target;
            _fsm.ChangeState("Preparing");
        }    
    }

    #region PrepareState
    //void PrepareTurret()
    //{
    //    if (target!=null && AlignBase(target.position) && AlignCanon(Vector3.up))
    //    {
    //        TurretUpdate += ShootState;
    //        TurretUpdate -= PrepareTurret;
    //    }
           

    //}

    public bool AlignBase(Vector3 _target)
    {
        Vector3 dir = _target - transform.position;
        Vector3 desiredForward = new Vector3(0, 0, dir.z);
        if (desiredForward.z >= dir.z) return true;


        pivotBase.forward += dir.normalized * Time.deltaTime * _baseRotationSpeed;
        return false;

    }

    public bool AlignCanon(Vector3 dir)
    {

        Vector3 desiredForward = dir * _canonRotationSpeed * Time.deltaTime;

        if (desiredForward.z >= _maxBatteryAngle) return true;

        pivotBase.forward += desiredForward;
        return false;
    }
    #endregion

    #region ShootState
    void ShootState()
    {
        if (target==null)     
            TurretUpdate -= ShootState;
        
      
        
        if (MisileCD())
        {
           
            misilesLeft--;
            if (misilesLeft<=0) TurretUpdate += VolleyCD;
        }
      
    }

    public void ShootMisile(Transform _target)
    {
        Transform shootPos = ActualShootPos();
        Instantiate(misilePrefab, shootPos.position, Quaternion.identity).Initialize(_misileStats, _target);
    }


    //tiempo entre misil y misil, se pregunta en un if
    bool MisileCD()
    {
        actualMisileCD -= Time.deltaTime;
        if (actualMisileCD <= 0)
        {
            actualMisileCD = cd_BetweenMisiles;
            return true;
        }
        return false;
    }



    //tiempo entre rafaga de misiles, se suma a un action
    void VolleyCD()
    {
        actualVolleyCD -= Time.deltaTime;
        if (actualVolleyCD > 0) return;

        actualVolleyCD = cd_Volleys;
        TurretUpdate -= VolleyCD;

    }

    public Transform ActualShootPos() 
    => _shootPositions.Skip(UnityEngine.Random.Range(0, _shootPositions.Length)).First();
    #endregion

    #region Idle

    void StartGoingIdle()
    {
        if (AlignBase(Vector3.zero)&&AlignCanon(Vector3.zero))
        {
            TurretUpdate += Idle;
            TurretUpdate -= StartGoingIdle;
        }
    }

    void Idle()
    {
        AlignBase(Vector3.right);
    }


    #endregion

}
