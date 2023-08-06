using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Muzzle : Attachment
{
    [Space(5f)]
    [Header("Muzzle Variables")]
    //estaria bueno despues hacer esto una propiedad,
    //no lo cambio porque reasignar cosas en el editor
    [SerializeField]Transform _shootPos;
    public Transform shootPos => _shootPos;

    

    protected override void Initialize()
    {
        
    }
    protected override void Comunicate()
    {
        
    }

}
