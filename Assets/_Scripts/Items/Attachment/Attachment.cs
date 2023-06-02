using AYellowpaper.SerializedCollections;
using FacundoColomboMethods;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static StatsHandler;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(VFX_Sign))]
public abstract class Attachment : MonoBehaviour
{

    // queria que la variable de "value" tuviera un range,
    // pero no era posible si no estaba en un struct
    public string TESTNAME;
    [System.Serializable]
    public struct StatChangeParams
    {
        [Range(-100,100),SerializeField] int _value;
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
        TriggerHandler

    }

    [SerializeField] Transform pivotPos;
    [SerializeField] protected AttachmentType _myType;
    public AttachmentType myType => _myType;
    //protected AttachmentStats _stats;

    //se le pueden pasar valores negativos para que alguna estadistica disminuya.
    [SerializeField, SerializedDictionary("Stat Name", "Value To Add/Substract")]
    public SerializedDictionary<StatNames, StatChangeParams> Attachment_stats;
    //SerializedDictionary<StatNames, int> _stats= new SerializedDictionary<StatNames, int>();
    
    protected bool _isAttached;
    public bool isAttached => _isAttached;

    protected event Action onAttach;
    protected event Action onDettach;

    public Gun owner { get; private set; }

    LineRenderer sign;

    Tuple<Vector3, Quaternion> OriginPivot; 
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
        b_collider = GetComponent<BoxCollider>();
        Action<bool> colliderEnable = (x) => b_collider.enabled = x;
        onAttach += () => colliderEnable(false);
        onDettach += () => colliderEnable(true);
        _isAttached = false;
        #region Pivot

        pivotPos = pivotPos == null ? transform : pivotPos;

        if (pivotPos != transform)       
            OriginPivot = Tuple.Create(transform.position-pivotPos.position, pivotPos.rotation);        
        else        
            OriginPivot = Tuple.Create(Vector3.zero, new Quaternion(0,0,0,0));

        #endregion

        colliderEnable(!isAttached);

        Initialize();
        
    }

    protected virtual void Start()
    {

        gameObject.layer = AttachmentManager.instance.attachmentLayer.LayerBitmapToInt();    
        Comunicate(); SetVFXsign();   
        if (isAttached) onAttach?.Invoke(); else onDettach?.Invoke();
        
       
    }
   
    void SetVFXsign()
    {
        VFX_Sign a = GetComponent<VFX_Sign>();
        onAttach += a.DeactivateSign;
        onDettach += a.ActivateSign;

    }

 

    // hace que el accesorio se vuelva hijo del arma y le añada sus estadisticas
    public void Attach(Gun gun,Transform AttachTo)
    {
    
        if (gun == null) return;      
        transform.parent = AttachTo;

         pivotPos.position = AttachTo.position;
         pivotPos.rotation = AttachTo.rotation;

        if (pivotPos != transform)
        {
            transform.position = pivotPos.position + OriginPivot.Item1;
            transform.rotation = pivotPos.rotation * OriginPivot.Item2;
        }        
         owner = gun;
         _isAttached = true;

         owner.stats.ChangeStats(Attachment_stats, true);
         onAttach?.Invoke();       
    }

    // para cuando las armas tengan colliders
    Vector3 GetMeshCollision(Transform pivot)
    {
        return Physics.Raycast(pivot.position, Vector3.down, out RaycastHit hit) ? hit.point : pivot.position;
       
    }
    //saca sus stats del arma y queda sin padre
    public void Dettach()
    {
        if (!_isAttached) return;
        owner.stats.ChangeStats(Attachment_stats, false);

        owner=null;
        this.transform.parent=null;
        _isAttached=false;

        onDettach?.Invoke();
       
    }

    private void OnDrawGizmos()
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
