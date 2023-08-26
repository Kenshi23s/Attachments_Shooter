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
        if(Input.GetKeyDown(KeyCode.P) && canTouchSpace) 
        { 
            StartCoroutine("TurnOnRadar");
            Debug.Log("Empezó la corrutina");
        }
    }

    IEnumerator TurnOnRadar()
    {
        field.gameObject.SetActive(true);
        canTouchSpace = false;

        while(!inoffBig)
        {
            field.transform.localScale += Vector3.one*Time.deltaTime*increaseSpeed;
            Debug.Log("Estoy aumentando la escala");

            if (field.transform.localScale.x >= preferedFieldSize)
            {
                inoffBig = true;
            }

            yield return null;
        }
        
        yield return new WaitForSeconds(5);
        
        field.transform.localScale = Vector3.one;
        field.gameObject.SetActive(false);
        Debug.Log("Volvio a la normalidad");

        yield return new WaitForSeconds(waitingTime);

        canTouchSpace = true;
        inoffBig=false;
        Debug.Log("Puedo volver a activar radar");

    }
}
