using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static StatsHandler;

public abstract class Attachment : MonoBehaviour
{
    [System.Serializable]
    // queria que la variable de "value" tuviera un range,
    // pero no era posible si no estaba en un struct
   public struct StatChangeParams
   {

        [Range(-100,100),SerializeField] int _value;
        public int value => _value;

        #region Obsolete
        //[SerializeField] bool isMinus;
        //private int mySignValue;
        //public void SetSign()  => mySignValue = isMinus == true ? -1 : 1;
        #endregion
   }

    public enum AttachmentType
    {
        Muzzle,
        Magazine,
        Sight,
        Grip,
        LaserSight

    }

    [SerializeField] protected AttachmentType _myType;
    public AttachmentType myType => _myType;
    //protected AttachmentStats _stats;

    //se le pueden pasar valores negativos para que alguna estadistica disminuya.
    [SerializeField, SerializedDictionary("Stat Name", "Value To Add/Substract")]
    public SerializedDictionary<StatNames, StatChangeParams> Attachment_stats;
    //SerializedDictionary<StatNames, int> _stats= new SerializedDictionary<StatNames, int>();
    
    bool _isAttached;
    public bool isAttached => _isAttached;

    protected event Action OnAtach;
    protected event Action OnUnAttach;

    protected GunFather gunAttachedTo;

    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize() { }

    //void SetStatsSign()
    //{
    //    foreach (StatNames key in Enum.GetValues(typeof(StatNames)))      
    //        if (Attachment_stats.ContainsKey(key))
    //                 Attachment_stats[key].SetSign();
            

        
    //}

    // hace que el accesorio se vuelva hijo del arma y le añada sus estadisticas
    public void Attach(GunFather gun,Transform parent,Vector3 Pos)
    {
        if (gun!=null)
        {
            this.transform.parent = parent;
            transform.position = Pos;
            gunAttachedTo = gun;
            _isAttached = true;

            gunAttachedTo.stats.ChangeStats(Attachment_stats, true);
            OnAtach?.Invoke();
        }
        
    }

    //saca sus stats del arma y queda sin padre
    public void UnAttach()
    {
        gunAttachedTo.stats.ChangeStats(Attachment_stats, false);

        gunAttachedTo=null;
        this.transform.parent=null;
        _isAttached=false;

        OnUnAttach?.Invoke();
       
    }
   
}
