using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjetive : MonoBehaviour
{
    [SerializeField] GameObject field;
    [SerializeField] private float preferedFieldSize;
    [SerializeField] float increaseSpeed = 1;
    [SerializeField] float waitingTime = 5;

    private bool canTouchSpace = true;
    private bool inoffBig = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && canTouchSpace)
        {
            StartCoroutine("TurnOnRadar");
            Debug.Log("Empezó la corrutina");
        }
    }


}
