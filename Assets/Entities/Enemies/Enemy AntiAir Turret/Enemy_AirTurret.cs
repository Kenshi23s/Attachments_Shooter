using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using Unity.Mathematics;
using FacundoColomboMethods;

public class Enemy_AirTurret : Enemy,IDetector
{
    //aca me guardo el target actual
    public Transform target => _target;
   [SerializeField] Transform _target;
  
    #region Pivots
    //x                   //y //pensar mejores nombres
    [Header("Base Pivot"), Space]
    [SerializeField] Transform pivotBase;
    [SerializeField] float _baseRotationSpeed;

    [Header("Misile Battery Pivot"), Space]
    [SerializeField] Transform pivotMisileBattery;
    [SerializeField, Range(0, 50f)] float _maxBatteryAngle;
    [SerializeField] float _canonRotationSpeed;
 

    [Space,SerializeField] Transform[] _shootPositions;
    #endregion

    #region Misile
    [Header("Misile"), Space(10f)]
   
    [SerializeField]TurretMisile.MisileStats _misileStats;
 
    [SerializeField, Space] TurretMisile misilePrefab;
    [SerializeField,Range(1,30)] int misilesPerVolley;
    #endregion

    #region Cooldowns
    [Header("Cooldowns"), Space]
    [SerializeField,Tooltip("El tiempo entre misil y misil en una rafaga de misiles"),Range(0.1f,1)] 
    float cd_BetweenMisiles;
    [SerializeField,Tooltip("El tiempo entre rafaga de misiles y rafaga de misiles"),Range(3, 10)]  
    float cd_Volleys;


  
    #endregion
   
    #region Solo Lectura en editor
    //desp hacerle una pool
    [Header("VARIABLES DE SOLO LECTURA"), Space]
    [SerializeField]float actualMisileCD;
    [SerializeField]float actualVolleyCD;
    [SerializeField]float misilesLeft;
    [SerializeField] bool button = false;
    #endregion


    //[SerializeField]Transform[] _batteries;
    StateMachine<string> _fsm;

    private void Awake()
    {
        SetTurretFSM();
      
        //SetTurretFSM();
    }

    //void ChangMaxAngleValue()
    //{
    //    _maxBatteryAngle *= -1;
    //}
    void SetTurretFSM()
    {
        _fsm = new StateMachine<string>();
        _fsm.CreateState("Idle", new AirTurretState_Idle(this,pivotBase));
        _fsm.CreateState("Align", new AirTurretState_Align(this, _fsm));
        _fsm.CreateState("Shoot", new AirTurretState_Shoot(this,cd_BetweenMisiles,cd_Volleys,misilesPerVolley, _fsm));
        _fsm.CreateState("Rest",  new AirTurretState_Rest(this, _fsm));
        
        _fsm.ChangeState("Idle");
       
    }

    public void OnRangeCallBack(Player_Movement item)
    {
        TargetDetected(item.transform);
    }

    public void OutOfRangeCallBack(Player_Movement item)
    {
        _target = null;
    }
    private void Update()
    {
        _fsm.Execute();
    }

    //tendria q hacer otra clase q haga un callback hacia esta?
    void TargetDetected(Transform target)
    {
        if (target!=null && _fsm.actualState!= "Shoot")
        {
            this._target = target;
            _fsm.ChangeState("Align");
        }          
    }

    #region AlignMethods
  

    public bool AlignBase(Vector3 _target)
    {
        Vector3 dir = _target - pivotBase.position;
        Vector3 desiredForward = new Vector3(dir.x, 0, dir.z).normalized * Time.deltaTime * _baseRotationSpeed;
        pivotBase.forward += desiredForward;
        Debug.Log("Aligning Base...");
        float angle = Vector3.Angle(pivotBase.forward, desiredForward.normalized);
     
        if (angle > 5) return false;
        Debug.Log("Base Aligned...");
        return true;

    }

    //true es para mover hacia arriba
    //false para mover hacia abajo
    public bool AlignCanon(bool goUp)
    {

        if (CheckIFMaxAngle(goUp))
         return true;
        Debug.Log("Aligning Canon...");
        Vector3 AlignDir = goUp ? Vector3.left : Vector3.right;
        pivotMisileBattery.eulerAngles += AlignDir * _canonRotationSpeed * Time.deltaTime;

        return CheckIFMaxAngle(goUp);
    }

    bool CheckIFMaxAngle(bool dir)
    {

        //si esta en alguno de los limites devuelvo true(deberia setearlos a su limite por las dudas?)

        //por ejemplo, se paso de 0 por un poco, lo seteo a 0?
        //esto es legible?, creo que no. Consultar a alguien

        //la rotacion es en negativo, pero euler angles por codigo da positivo, maldigo unity
                                                     // si mi angulo esta entre (360 - angulo deseado) y 180
                                                     //esto lo hago porque euler angles va de 0 a 360
                                                     //ej, si mi angulo esta entre 360 - 36 = 324 y 180, lo clampeo 
        if (dir && pivotMisileBattery.localEulerAngles.x.InBetween(360 - _maxBatteryAngle, 180))
        {                                                // unity en editor opera con negativo
            pivotMisileBattery.eulerAngles = new Vector3(-_maxBatteryAngle, pivotMisileBattery.eulerAngles.y, 0);
            Debug.Log("Canon Aligned!");
            return true;          
          
        }
        //harcodeado mal D:, despues fijarse como obtener el valor negativo
        else if(!dir && pivotMisileBattery.eulerAngles.x.InBetween(10, 0))
        {
           Debug.Log(pivotMisileBattery.eulerAngles.x+"es mayor a 0");
           pivotMisileBattery.eulerAngles = Vector3.zero;
           return true;          
        }
   
        return false;


    }

    

    public Transform ActualShootPos()
    => _shootPositions.Skip(UnityEngine.Random.Range(0, _shootPositions.Length)).First();
    #endregion

    public void ShootMisile(Transform _target)
    {
        Transform shootPos = ActualShootPos();
        Instantiate(misilePrefab, shootPos.position, Quaternion.identity).Initialize(_misileStats, _target, shootPos.forward);
        Debug.Log("CallTrack");
    }


    private void OnDrawGizmos()
    {

        if (target!=null)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
        Gizmos.color = Color.blue;
        Vector3 MaxAnglePoint = pivotMisileBattery.position + new Vector3(0, _maxBatteryAngle, 0);

        Gizmos.DrawLine(pivotMisileBattery.position, pivotMisileBattery.position + MaxAnglePoint.normalized * 12);

        Gizmos.DrawLine(transform.position, transform.position + transform.right * 12);
        //union de mis 2 puntos
        Gizmos.DrawLine(pivotMisileBattery.position + MaxAnglePoint.normalized * 12, transform.position + transform.right * 12);
    }

    public override void Pause()
    {
        throw new NotImplementedException();
    }

    public override void Resume()
    {
        throw new NotImplementedException();
    }

    protected override int OnTakeDamage(int dmgDealt)
    {
        return dmgDealt;
    }

    public override bool WasCrit() => false;

  



    //private void OnValidate()
    //{
    //    pivotMisileBattery.localEulerAngles = new Vector3(0, 0, _maxBatteryAngle);
    //}

}
