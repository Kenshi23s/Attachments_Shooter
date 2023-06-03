using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraPivots : MonoSingleton<PlayerCameraPivots>
{
   [SerializeField] Transform _viewFromInventory;
    public Transform ViewFromInventory => _viewFromInventory;

    protected override void SingletonAwake()
    {
       
        enabled = false;
    }
   
  
}
