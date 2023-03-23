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

    protected AttachmentType myType;
    //protected AttachmentStats _stats;
    [SerializeField,SerializedDictionary("Stat Name","Value")]
    SerializedDictionary<StatNames, int> _stats;
    
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

            gunAttachedTo._stats.ChangeStats(_stats, true);
            OnAtach?.Invoke();
        }
        
    }

    public void UnAttach()
    {
        gunAttachedTo._stats.ChangeStats(_stats, false);
        OnUnAttach?.Invoke();
       
    }
   
}
