using System;
using UnityEngine;
using System.Linq;
using FacundoColomboMethods;

public class Enemy_AirTurret : Enemy, IDetector
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
    [Tooltip("donde estarian los ojos de la torreta para /VER/ al player ")]
    [SerializeField] public Transform turretEYE;
 

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
  

    public float ActualMisileCD { get => actualMisileCD; set => actualMisileCD = value; }
    public float ActualVolleyCD { get => actualVolleyCD; set => actualVolleyCD = value; }
    public float MisilesLeft { get => misilesLeft; set => misilesLeft = value; }

    float actualMisileCD;
     float actualVolleyCD;
     float misilesLeft;
    #endregion
    //lo tendria q tener el game manager
    public LayerMask wallMask;
    //[SerializeField]Transform[] _batteries;
    StateMachine<string> _fsm;

    void Awake()
    {    
        _misileStats.owner = this;
        GetComponent<LifeComponent>().OnKilled += () => Destroy(gameObject);
        debug=GetComponent<DebugableObject>();
        debug.AddGizmoAction(DrawGizmo);
        SetTurretFSM();
    }

    void SetTurretFSM()
    {

        _fsm = new StateMachine<string>();
        _fsm.Initialize(debug);
        _fsm.CreateState("Idle",  new AirTurretState_Idle(this,pivotBase));
        _fsm.CreateState("Align", new AirTurretState_Align(this, _fsm));
        _fsm.CreateState("Shoot", new AirTurretState_Shoot(this,cd_BetweenMisiles, cd_Volleys, misilesPerVolley, _fsm));
        _fsm.CreateState("Rest",  new AirTurretState_Rest(this, _fsm));
        
        _fsm.ChangeState("Idle");
       
    }

    public void OnRangeCallBack(Player_Movement item) => TargetDetected(item.transform);

    public void OutOfRangeCallBack(Player_Movement item) => _target = null;

    private void Update()
    {
        _fsm.Execute();
    }

    //tendria q hacer otra clase q haga un callback hacia esta?
    void TargetDetected(Transform target)
    {
        if (target!=null && _fsm.actualState != "Shoot")
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
        float angle = Vector3.Angle(pivotBase.forward, desiredForward.normalized);
     
        if (angle > 5) return false;
      
        return true;

    }

    //true es para mover hacia arriba
    //false para mover hacia abajo
    public bool AlignCanon(bool goUp)
    {

        if (CheckIFMaxAngle(goUp))
         return true;
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
           
            return true;          
          
        }
        //harcodeado mal D:, despues fijarse como obtener el valor negativo
        else if(!dir && pivotMisileBattery.eulerAngles.x.InBetween(10, 0))
        {
          
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
    }


    private void DrawGizmo()
    {
        if (target!=null)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }            
    }
    






    //private void OnValidate()
    //{
    //    pivotMisileBattery.localEulerAngles = new Vector3(0, 0, _maxBatteryAngle);
    //}

}
