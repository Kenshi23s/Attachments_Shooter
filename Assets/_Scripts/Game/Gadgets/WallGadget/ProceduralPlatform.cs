using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;


//estaria bueno tener una pool para este tipo de objetos
//puede consumir bastante y trabar todo
//tambien deberian hacerce algunas cosas entre frames y no de golpe
//ahora estoy prototipando, pero en un fututo(apenas funcione bien esto)
//tendria que ponerme a optimizar

public class ProceduralPlatform : MonoBehaviour
{

    #region Directions
    //chat gpt :3 //no iba a escribir esto a mano
    //vivan las IAs que me sacaran mi trabajo
    //private static readonly Vector3[] AllDirections = new Vector3[]
    //{
    //      //direcciones
    //       Vector3.right,
    //       Vector3.up,
    //       Vector3.forward,
    //       -Vector3.right,
    //       -Vector3.up,
    //       -Vector3.forward,

    //       //diagonales
    //       Vector3.right + Vector3.up,
    //       Vector3.up + Vector3.forward,
    //       Vector3.forward + Vector3.right,
    //       Vector3.right - Vector3.up,
    //       Vector3.up - Vector3.forward,
    //       Vector3.forward - Vector3.right,
    //       Vector3.right - Vector3.forward,
    //       Vector3.up - Vector3.right,
    //       Vector3.forward - Vector3.up,
    //       -Vector3.right + Vector3.forward,
    //       -Vector3.up + Vector3.right,
    //       -Vector3.forward + Vector3.up,

    //       //doble diagonales
    //       Vector3.right + Vector3.up + Vector3.forward,
    //       Vector3.right - Vector3.up + Vector3.forward,
    //       Vector3.right + Vector3.up - Vector3.forward,
    //       -Vector3.right + Vector3.up + Vector3.forward,
    //       -Vector3.right - Vector3.up + Vector3.forward,
    //       -Vector3.right + Vector3.up - Vector3.forward,
    //       Vector3.right - Vector3.up - Vector3.forward,
    //       -Vector3.right - Vector3.up - Vector3.forward
    //};

    private static readonly Vector3[] SixDirections = new Vector3[]
    {
        Vector3.up,               //Atras
        Vector3.forward,         //Adelante
        Vector3.back,           // Arriba
        Vector3.down,          // Abajo
        Vector3.left,         // Izquierda
        Vector3.right,       // Derecha
    };
    #endregion

    [Serializable]
    public struct PlatformsParameters
    {
        [NonSerialized] public Vector3 CenterPosition, CrossResult;
        public LayerMask SolidMasks;
        public float wallSeparation,Radius, SeparationBetweenPlatforms, Slice, InitialLifeTime, DecayDelaySeconds;
        public int ConstructionDelay;

    }

    public bool IsFoundation { get; private set; }

    public event Action<ProceduralPlatform> OnFoundationDestroyed = delegate { };

    public event Action<ProceduralPlatform> OnNeighborDestroyed = delegate { };

    float _currentLifeTime, _maxLifeTime;
    public float _decayDelaySeconds { get; private set; }

    [SerializeField] GameObject _view;

    [SerializeField] Gradient _gradient;

    #region View
    private void Start()
    {
        //rotacion random para el view
        Vector3 rotation = Vector3.zero;
        for (int i = 0; i < 3; i++)    
            rotation[i] = Random.Range(0, 4) * 90;
        
        _view.transform.localRotation = Quaternion.Euler(rotation);
    }

    #endregion

    Material myMat;
    private void Awake()
    {
        myMat = _view.GetComponent<Renderer>().material;
        myMat.color = _gradient.Evaluate(1);     
    }

    private void LateUpdate()
    {
        _currentLifeTime -= Time.deltaTime;
        myMat.color = _gradient.Evaluate(_currentLifeTime / _maxLifeTime);
        if (_currentLifeTime <= 0) StoreNode();
    }

    void StoreNode()
    {
        var col = ProceduralPlatformManager.instance.GetNeighbors(this);
        ProceduralPlatformManager.instance.RemoveOwner(this);
        foreach (var item in col)
            item.RemoveNeighbor(this);

        ProceduralPlatformManager.instance.StorePlatform(this);
    }

    void RemoveNeighbor(ProceduralPlatform neighbor)
    {
        if (!ProceduralPlatformManager.instance.RemoveNode(this, neighbor)) return;

        if (!neighbor.IsFoundation) return;

        if (!AnyFoundationLeft(FList.Create(this))) ProceduralPlatformManager.instance.ChainStoreAll(this);

    }
    #region Recusrion Logic

    public bool AnyFoundationLeft(FList<ProceduralPlatform> alreadyChecked)
    {
        if (IsFoundation) return true;
        alreadyChecked += this;
        foreach (var item in ProceduralPlatformManager.instance.GetNeighbors(this).Where(x => !alreadyChecked.Contains(x)).Distinct())
        {

            if (item.AnyFoundationLeft(alreadyChecked))
                return true;

            alreadyChecked += item;
        }
        return false;
    }

    #endregion

    #region Creation Logic

    public async Task CreatePlatform(Vector3 position, PlatformsParameters parameters)
    {     
        transform.position = position;
        _view.transform.localPosition = Vector3.zero;
        IsFoundation = false;
        foreach (var dir in SixDirections)
        {
            if (!Physics.Raycast(transform.position, dir, _view.transform.localScale.magnitude, parameters.SolidMasks, QueryTriggerInteraction.Ignore)) continue;

            IsFoundation = true; break;
        }

        _maxLifeTime = _currentLifeTime = parameters.InitialLifeTime;

        _decayDelaySeconds = parameters.DecayDelaySeconds;
        await Task.Delay(parameters.ConstructionDelay);
        await Ramify(parameters);

    }

    async Task Ramify(PlatformsParameters parameters)
    {
        int count = 0;
        foreach (Vector3 direction in SixDirections)
        {

            var newPosition = transform.position + direction.normalized * parameters.SeparationBetweenPlatforms;
            if (!InDistance(parameters, newPosition)) continue;
            count++;
            Vector3 dirToMiddle = newPosition - parameters.CenterPosition;
            if (InSlice(parameters.CrossResult, dirToMiddle, parameters.Slice)) continue;

            var col = Physics.RaycastAll(transform.position, direction, parameters.SeparationBetweenPlatforms, parameters.SolidMasks)
                .Where(x => x.transform != transform)
                .ToArray();
          
            Debug.DrawLine(transform.position, transform.position + direction.normalized * parameters.SeparationBetweenPlatforms, Color.cyan, Mathf.Infinity);


            if (!col.Any())
            {   
                var newGO = ProceduralPlatformManager.instance.pool.Get();
                ProceduralPlatformManager.instance.AddNode(this, newGO);
                newGO.CreatePlatform(newPosition, parameters);
            }
            //else
            //{
              
            //    //esto esta bien feo lo del getComponent
            //    //es una de las razones por las que se nos lagueaba el tp de IA 2
            //    //pero no podes castear un raycast hit a proceduralplatform(creo)
            //    //consultar el martes
            //    var existingPlatform = col
            //        .Select(x => x.transform.GetComponent<ProceduralPlatform>())
            //        .Where(x => x != null)
            //        .First();
            //    //Debug.Log("Ya habia plataforma, asi q la hago mi vecino y no creo otra");
            //    if (existingPlatform != null && existingPlatform != default) 
            //    ProceduralPlatformManager.instance.AddNode(this, existingPlatform);
            //}


        }
        
    }
    #region Useful Questions
    bool InDistance(PlatformsParameters x, Vector3 newPos)
    {
        var distance = Vector3.Distance(x.CenterPosition, newPos);
        return distance <= x.Radius;
    }

    bool InSlice(Vector3 orientation, Vector3 DistanceFromCenter, float dist)
    {
        orientation = orientation.normalized;
        return new Vector3(
            orientation.x * DistanceFromCenter.x,
            orientation.y * DistanceFromCenter.y,
            orientation.z * DistanceFromCenter.z).magnitude > dist;
    }
    #endregion
    #endregion
    /// <summary>
    /// aca se deberia hacer toda la logica de volver a la pool y demas
    /// </summary>
    public void CleanObject()
    {
        OnFoundationDestroyed = delegate { };
        OnNeighborDestroyed = delegate { };
        IsFoundation = false;


    }

    #region UsefulMethods
  
   
    public static Vector3 GetPerpendicular(Vector3 dir, Vector3 wallNormal)
    {
      
        Vector3 WallPerpendicular = Vector3.Cross(dir.normalized, wallNormal);
       
        return Vector3.Cross(WallPerpendicular, wallNormal).normalized;
    }

    #endregion





}
