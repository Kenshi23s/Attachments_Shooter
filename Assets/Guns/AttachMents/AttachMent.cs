using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GunStats;

public abstract class Attachment : MonoBehaviour
{
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

    //se le pueden pasar valores negativos para que alguna estadistica disminuya
    [SerializeField, SerializedDictionary("Stat Name", "Value To Add/Substract")]
    public SerializedDictionary<StatNames, int> Attachment_stats;
    //SerializedDictionary<StatNames, int> _stats= new SerializedDictionary<StatNames, int>();

    public bool isAttached => _isAttached;
    bool _isAttached;

    protected event Action OnAtach;
    protected event Action OnUnAttach;

    protected GunFather gunAttachedTo;

    public void Attach(GunFather gun,Transform parent,Vector3 Pos)
    {
        if (gun!=null)
        {
            this.transform.parent = parent;
            transform.position = Pos;
            gunAttachedTo = gun;
            _isAttached = true;

            gunAttachedTo._stats.ChangeStats(Attachment_stats, true);
            OnAtach?.Invoke();
        }
        
    }

    public void UnAttach()
    {
        gunAttachedTo._stats.ChangeStats(Attachment_stats, false);

        gunAttachedTo=null;
        this.transform.parent=null;
        _isAttached=false;

        OnUnAttach?.Invoke();
       
    }
   
}
