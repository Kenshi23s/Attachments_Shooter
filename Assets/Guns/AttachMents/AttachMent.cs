using System;
using UnityEngine;

public abstract class Attachment : MonoBehaviour
{
    protected GunStats _atacchmentStats; 

    protected event Action OnAtach;
    protected event Action OnUnAttach;

    protected GunFather gunAttachedTo;

    private void Awake()
    {
        
    }

    public void Attach(GunFather gun,Transform parent,Vector3 Pos)
    {
        this.transform.parent = parent;
        transform.position = Pos;
        gunAttachedTo = gun;
        gunAttachedTo.Stats.ChangeStats(_atacchmentStats,true);
        OnAtach?.Invoke();
    }
    public void UnAttach()
    {
        gunAttachedTo.Stats.ChangeStats(_atacchmentStats, false);
        OnUnAttach?.Invoke();
        gameObject.SetActive(false);
    }
   
}
