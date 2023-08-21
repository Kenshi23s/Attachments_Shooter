using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShoot : MonoBehaviour
{
    [SerializeField] Projectile_Acid sample;

    private void Awake()
    {
         
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
          var z =  Instantiate(sample, Player_Handler.position+Vector3.up*30f, Quaternion.identity);
           
        }
    }
}
