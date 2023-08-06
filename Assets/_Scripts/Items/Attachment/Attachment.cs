using AYellowpaper.SerializedCollections;
using FacundoColomboMethods;
using System;
using UnityEngine;
using UnityEngine.UI;
using static StatsHandler;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(VFX_Attachment))]
public abstract class Attachment : MonoBehaviour
{
    protected DebugableObject _debug;
    // queria que la variable de "value" tuviera un range,
    // pero no era posible si no estaba en un struct
    #region DataStructures
    [System.Serializable]
    public struct StatChangeParams
    {
        [Range(-100, 100), SerializeField] int _value;
        public int value => _value;
    }

    public enum AttachmentType
    {
        Muzzle,
        Magazine,
        Sight,
        Grip,
        LaserSight,
        Stock,
        RearGrip

    }
    #endregion

    [SerializeField] Transform pivotPos;

    public AttachmentType MyType { get; protected set; }

    
    //protected AttachmentStats _stats;

    //se le pueden pasar valores negativos para que alguna estadistica disminuya.
    [SerializeField, SerializedDictionary("Stat Name", "Value To Add/Substract")]
    public SerializedDictionary<StatNames, StatChangeParams> Attachment_stats;
    //SerializedDictionary<StatNames, int> _stats= new SerializedDictionary<StatNames, int>();


    public bool isAttached { get; protected set; }

    public Gun owner { get; private set; }
    public VFX_Attachment VFX { get; private set; }

    public event Action OnAttach;
    public event Action OnDettach;

    Vector3 OriginPivot = Vector3.zero; 
    BoxCollider b_collider;
    
    /// <summary>
    /// Awake
    /// </summary>
    protected abstract void Initialize();
    /// <summary>
    /// Start
    /// </summary>
    protected abstract void Comunicate();

    private void Awake()
    {     
        _debug = GetComponent<DebugableObject>(); _debug.AddGizmoAction(DrawPivot);
        VFX = GetComponent<VFX_Attachment>(); VFX.Initialize();
        isAttached = false;
        #region Pivot
        
        pivotPos = pivotPos == null ? transform : pivotPos;

        if (pivotPos != transform)
        {
            OriginPivot = pivotPos.localPosition;
            _debug.Log(OriginPivot.ToString());
        }
        #endregion

         SetColider(); Initialize();       
    }
    #region VFX Attachment(Talvez habria que crear otra clase para la logica de VFX)
   


    #endregion

    void SetColider()
    {
        b_collider = GetComponent<BoxCollider>();
        Action<bool> colliderEnable = (x) => b_collider.enabled = x;
        OnAttach += () => colliderEnable(false);
        OnDettach += () => colliderEnable(true);
        colliderEnable(!isAttached);
    }

    protected virtual void Start()
    {
        gameObject.layer = AttachmentManager.instance.AttachmentLayer.LayerMaskToLayerNumber();    
        Comunicate();    
        if (isAttached) OnAttach?.Invoke(); else OnDettach?.Invoke();       
    }
   
   

    // hace que el accesorio se vuelva hijo del arma y le añada sus estadisticas
    public void Attach(Gun gun,Transform AttachTo)
    {
    
        if (gun == null) return;
     
         transform.parent = AttachTo;

        if (pivotPos != transform)
        {
            _debug.Log("Tengo pivot!, lo uso para posicionarme");
            //Vector3 savePos = pivotPos.position + OriginPivot.Item1;

            transform.rotation = AttachTo.rotation;

            transform.position =  AttachTo.position - OriginPivot.GetOrientedVector(AttachTo);

            //transform.forward *= pivotPos.forward.z;
            //transform.right *= pivotPos.right.x;
            //transform.up *= pivotPos.up.y;
        }
        else
        {
            pivotPos.position = AttachTo.position;
            pivotPos.rotation = AttachTo.rotation;
        }
        
         owner = gun;
         isAttached = true;

         owner.stats.ChangeStats(Attachment_stats, true);
         OnAttach?.Invoke();       
    }

    // para cuando las armas tengan colliders
    Vector3 GetMeshCollision(Transform pivot)
    {
        return Physics.Raycast(pivot.position, Vector3.down, out RaycastHit hit) ? hit.point : pivot.position;      
    }

    //saca sus stats del arma y queda sin padre
    public void Dettach()
    {
        if (!isAttached) return;
        owner.stats.ChangeStats(Attachment_stats, false);

        owner = null; transform.parent = null;
        isAttached = false;

        OnDettach?.Invoke();      
    }

    public void DrawPivot()
    {
        if (pivotPos != null) 
        {
            Gizmos.DrawWireCube(pivotPos.position, new Vector3(0.01f, 0.01f, 0.01f));
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(0.01f, 0.01f, 0.01f));
        }     
    }
}
