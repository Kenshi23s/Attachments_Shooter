using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Muzzle : Attachment
{
    [Space(5f)]
    [Header("Muzzle Variables")]
    [SerializeField]Transform _shootPos;
    public Transform shootPos => _shootPos;

    

    protected override void Initialize()
    {
        
    }
    protected override void Comunicate()
    {
        
    }
    //private void Awake()
    //{
    //    OnAtach += () => { gunAttachedTo?.Stats.AddRange(rangeStat);};

    //    OnUnAttach += () => { gunAttachedTo?.Stats.AddRange(-rangeStat);};
    //}

}
