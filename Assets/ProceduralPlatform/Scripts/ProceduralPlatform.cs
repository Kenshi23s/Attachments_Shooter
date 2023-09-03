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
    public static int WatchDog = 200;
    #region Directions
    //chat gpt :3 //no iba a escribir esto a mano
    //vivan las IAs que me sacaran mi trabajo
    private static readonly Vector3[] AllDirections = new Vector3[]
    {
          //direcciones
           Vector3.right,
           Vector3.up,
           Vector3.forward,
           -Vector3.right,
           -Vector3.up,
           -Vector3.forward,

           //diagonales
           Vector3.right + Vector3.up,
           Vector3.up + Vector3.forward,
           Vector3.forward + Vector3.right,
           Vector3.right - Vector3.up,
           Vector3.up - Vector3.forward,
           Vector3.forward - Vector3.right,
           Vector3.right - Vector3.forward,
           Vector3.up - Vector3.right,
           Vector3.forward - Vector3.up,
           -Vector3.right + Vector3.forward,
           -Vector3.up + Vector3.right,
           -Vector3.forward + Vector3.up,

           //doble diagonales
           Vector3.right + Vector3.up + Vector3.forward,
           Vector3.right - Vector3.up + Vector3.forward,
           Vector3.right + Vector3.up - Vector3.forward,
           -Vector3.right + Vector3.up + Vector3.forward,
           -Vector3.right - Vector3.up + Vector3.forward,
           -Vector3.right + Vector3.up - Vector3.forward,
           Vector3.right - Vector3.up - Vector3.forward,
           -Vector3.right - Vector3.up - Vector3.forward
    };

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

    [System.Serializable]
    public struct PlatformsParameters
    {
        [NonSerialized] public Vector3 CenterPosition, CrossResult;
       
        public float Radius, SeparationBetweenPlatforms,Slice;
        public int ConstructionDelay;
        public Vector3 CubeArea => Vector3.one * Radius;

    }
    Action onGizmoDraw = delegate { };

    public List<ProceduralPlatform> Neighbors = new List<ProceduralPlatform>();

    public List<ProceduralPlatform> Foundations = new List<ProceduralPlatform>();

    public bool IsFoundation { get; private set; }

    public event Action<ProceduralPlatform> OnFoundationDestroyed = delegate { };

    public event Action<ProceduralPlatform> OnNeighborDestroyed = delegate { };

    #region Recusrion Logic
    public void SetNeighbors(List<ProceduralPlatform> NewNeighbors)
    {
        //esto (creo) me genera una nueva instancia de la lista y no es una referencia
        Neighbors = NewNeighbors.ToList();

        foreach (var item in Neighbors)
        {
            item.OnNeighborDestroyed += RemoveNeighbor;
        }

        //los divido en 2 colecciones, los que son Fundaciones, y los que no son Fundaciones
        //(recien cuando lo termine me di cuenta q no nesecito dividirlos en 2 colecciones)
        //por ahora lo dejo asi para prototipar, luego volvere...
        var Split = Neighbors
            .Where(x => x != this)
            .SelectMany(x => x.Neighbors)
            .Distinct()
            .ToLookup(x => x.IsFoundation);

        //si no hay fundaciones entre mis vecinos y yo no soy una fundacion
        if (!Split[true].Any() && !IsFoundation)
        {
            //me destruyo, al no haber fundaciones no tiene sentido seguir(porque se esta cayendo todo abajo)
            OnNeighborDestroyed(this);
            Destroy(gameObject);
        }

        //cuando las fundaciones se destruyan, quiero que me avisen
        foreach (var item in Split[true])
        {
            item.OnFoundationDestroyed += RemoveFoundation;
        }
    }


    //esto despues lo podria tener con async
    void RemoveFoundation(ProceduralPlatform oldFoundation)
    {
        if (Foundations.Contains(oldFoundation))
        {
            Foundations.Remove(oldFoundation);
        }

        if (!Foundations.Any())
        {
            OnNeighborDestroyed(this);
            CleanObject();
            Destroy(gameObject);
        }
    }

    void RemoveNeighbor(ProceduralPlatform oldNeighbor)
    {
        if (Neighbors.Contains(oldNeighbor))
        {
            Neighbors.Remove(oldNeighbor);
        }

        if (!Neighbors.Any())
        {
            OnNeighborDestroyed(this);
            CleanObject();
            Destroy(gameObject);
        }
    }


    void AddFoundation(ProceduralPlatform newFoundation)
    {

    }

    void AddNeighbors(ProceduralPlatform NewNeighbor)
    {

    }

    #endregion

    #region Creation Logic

    [SerializeField] GameObject _view;
    #region View
    private void Start()
    {
        //rotacion random para el view
        
        _view.transform.rotation =  Quaternion.Euler(Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90);
    }

    #endregion

    public async Task CreatePlatform(Vector3 position, PlatformsParameters parameters)
    {

        


        //var direction = hitWith.bounds.center - transform.position;
        float RemainingRadius = parameters.Radius - Vector3.Distance(parameters.CenterPosition, position);


        transform.position = position;

        //Debug.DrawLine(position, position + parameters.CrossResult * 20, Color.green, Mathf.Infinity);
        onGizmoDraw += () =>
        {
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawWireSphere(position, parameters.SeparationBetweenPlatforms);
        };
        await Task.Delay(parameters.ConstructionDelay);
        await Ramify(parameters);

        Destroy(gameObject, parameters.ConstructionDelay* 1000);
    }

    async Task Ramify(PlatformsParameters parameters)
    {
       
        foreach (Vector3 direction in SixDirections)
        {

            var newPosition = transform.position + direction * parameters.SeparationBetweenPlatforms;
            if (!InDistance(parameters, newPosition)) continue;

            //agrege el slice, pero por razones de la vida todavia no funciona bien
            if (InSlice(parameters.CrossResult, newPosition - parameters.CenterPosition, parameters.Slice)) continue;

            var col = Physics.RaycastAll(transform.position, direction, parameters.SeparationBetweenPlatforms).Where(x => x.transform != this.transform).ToArray();
            Debug.DrawLine(transform.position, transform.position + direction.normalized * parameters.SeparationBetweenPlatforms, Color.cyan, Mathf.Infinity);


            if (col.Any()) continue;

            var newGO = Instantiate(this, newPosition, Quaternion.identity);

            newGO.name = WatchDog.ToString();
            
             newGO.CreatePlatform(newPosition, parameters);

        }

    }


    bool InDistance(PlatformsParameters x, Vector3 newPos)
    {
        var distance = Vector3.Distance(x.CenterPosition, newPos);
        return distance <= x.Radius;
    }

    bool InSlice(Vector3 orientation, Vector3 DistanceFromCenter, float dist)
    {
        orientation = orientation.normalized;
        return new Vector3(orientation.x * DistanceFromCenter.x,orientation.y * DistanceFromCenter.y,orientation.z* DistanceFromCenter.z).magnitude > dist;
    }


    #endregion
    /// <summary>
    /// aca se deberia hacer toda la logica de volver a la pool y demas
    /// </summary>
    void CleanObject()
    {
        Neighbors.Clear(); Foundations.Clear();
        OnFoundationDestroyed = delegate { };
        OnNeighborDestroyed = delegate { };
        IsFoundation = false;
        onGizmoDraw = delegate { };
    }

    private void OnDrawGizmos()
    {

        onGizmoDraw?.Invoke();

    }

    public List<ProceduralPlatform> GetNeighbors()
    {
        List<ProceduralPlatform> neighbors = new List<ProceduralPlatform>();
        for (int i = 0; i < AllDirections.Length; i++)
        {
            if (!Physics.Raycast(transform.position, AllDirections[i], out var hit)) continue;
            if (!hit.transform.TryGetComponent(out ProceduralPlatform x)) continue;
            neighbors.Add(x);
        }

        return Neighbors;
    }

    #region UsefulMethods
    public static Vector3 OrientVector(Transform tr, Vector3 dir)
    {
        return new Vector3(tr.right.x * dir.x, tr.up.y * dir.y, tr.forward.z * dir.z);
    }

    public static Vector3 GetPerpendicular(Vector3 dir, Vector3 wallNormal)
    {
        Vector3 firstPerpendicular = Vector3.Cross(dir.normalized, wallNormal);
        return Vector3.Cross(firstPerpendicular, wallNormal).normalized;
    }


    #endregion





}
