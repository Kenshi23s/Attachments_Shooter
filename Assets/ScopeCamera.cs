using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeCamera : MonoBehaviour
{


   
    private void Update()
    {
       
        if (ShakeCamera.aiming)
        {

            transform.position = Vector3.Lerp(transform.position, Camera.main.transform.position, Time.deltaTime);
            transform.forward= Camera.main.transform.forward;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime);
        }
    }
}
