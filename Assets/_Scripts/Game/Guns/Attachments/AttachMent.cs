using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static StatsHandler;
[RequireComponent(typeof(BoxCollider))]

public abstract class Attachment : MonoBehaviour
{
    [System.Serializable]
    // queria que la variable de "value" tuviera un range,
    // pero no era posible si no estaba en un struct
   
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
        Handler

    }

    [SerializeField] Transform pivotPos;
    [SerializeField] protected AttachmentType _myType;
    public AttachmentType myType => _myType;
    //protected AttachmentStats _stats;

    //se le pueden pasar valores negativos para que alguna estadistica disminuya.
    [SerializeField, SerializedDictionary("Stat Name", "Value To Add/Substract")]
    public SerializedDictionary<StatNames, StatChangeParams> Attachment_stats;
    //SerializedDictionary<StatNames, int> _stats= new SerializedDictionary<StatNames, int>();
    
    bool _isAttached;
    public bool isAttached => _isAttached;

    protected event Action onAttach;
    protected event Action onDettach;

    protected Gun gunAttachedTo;

  
    Tuple<Vector3, Quaternion> OriginPivot; 
    BoxCollider b_collider;

    private void Awake()
    {
        b_collider = GetComponent<BoxCollider>();
        Action<bool> colliderEnable = (x) => b_collider.enabled = x;
        onAttach += () => colliderEnable(false);
        onDettach += () => colliderEnable(true);

        #region Pivot

        pivotPos = pivotPos != null ? pivotPos : transform;

        if (pivotPos != transform)       
            OriginPivot = Tuple.Create(transform.position-pivotPos.position, pivotPos.rotation);        
        else        
            OriginPivot = Tuple.Create(Vector3.zero, new Quaternion(0,0,0,0));

        #endregion

        colliderEnable(!isAttached);

        Initialize();
        
    }

    private void Start()
    {
        gameObject.layer = AttachmentManager.instance.attachmentLayer;
    }

    protected abstract void Initialize();

    // hace que el accesorio se vuelva hijo del arma y le añada sus estadisticas
    public void Attach(Gun gun,Transform AttachTo)
    {
        if (gun == null) return;
        
         transform.parent = AttachTo;

         pivotPos.position = AttachTo.position;
         pivotPos.rotation = AttachTo.rotation;


        if (pivotPos!=this.transform)
        {
            transform.position = pivotPos.position + OriginPivot.Item1;
            transform.rotation = pivotPos.rotation * OriginPivot.Item2;
        }        

         gunAttachedTo = gun;
         _isAttached = true;

         gunAttachedTo.stats.ChangeStats(Attachment_stats, true);
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
        gunAttachedTo.stats.ChangeStats(Attachment_stats, false);

        gunAttachedTo=null;
        this.transform.parent=null;
        _isAttached=false;

        onDettach?.Invoke();
       
    }
   
}
