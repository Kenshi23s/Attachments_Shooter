using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShoot : MonoBehaviour
{
    Projectile_Acid sample;

    private void Awake()
    {
         
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
          var z =  Instantiate(sample, Player_Movement.position+Vector3.up*30f, Quaternion.identity);
           
        }
    }
}
