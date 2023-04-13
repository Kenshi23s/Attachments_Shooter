using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StandardElevator : MonoBehaviour
{
    Vector3 _originalPos;
    Vector3 objective;

    public KeepObjectsInside myKeepObjects;

    public Vector3 otherPos;
    public float velocity;
    public bool _activated;
    public float margen;

    [Range(0,1)]
    public float minForce;

    //para testear sin poner inputs al jugador (luego se borra)
    [Header("luego se borra")]
    public bool boton;

    bool _executable;

    public delegate bool myActions(Transform pos);
    public myActions myOrders;

    void Start()
    {
        _originalPos = transform.position;
    }


    void Update()
    {
        if (boton)
        {
            //intercambia el estado
            _activated = !_activated;

            if (_activated)
            {
                objective = _originalPos + otherPos;
            }
            else
            {
                objective = _originalPos;
            }

            myOrders = new myActions(MovePlataform);

            boton = false;
        }

        if (myOrders != null)
        {
            if (myOrders.Invoke(this.transform))
            {
                myOrders = null;
            }
        }
    }

    bool MovePlataform(Transform myTransform)
    {
        //supero la altura
        if ((myTransform.position - objective).magnitude < (velocity * Time.deltaTime) / margen)
        {
            myTransform.position = objective;
            return true;
        }

        Vector3 temp = (objective - myTransform.position).normalized * velocity * Time.deltaTime * SoftVelocity(myTransform,margen);

        myTransform.position += temp;

        if (myKeepObjects)
        {
            myKeepObjects.velocity = temp;
        }

        return false;
    }

    float SoftVelocity(Transform mytransform,float margen)
    {
        float myDistance = (mytransform.position - objective).magnitude;

        if (myDistance >= margen && myDistance <= otherPos.magnitude - margen)
        {
            return 1;
        }

        float min = Mathf.Clamp((myDistance / margen) + minForce, 0, 1);
        float max = Mathf.Clamp((((otherPos.magnitude) - myDistance) / margen) + minForce, 0, 1);

        Debug.Log(Mathf.Min(min, max));

        return Mathf.Min(min,max);
    }

    private void OnDrawGizmos()
    {

        if (_originalPos != Vector3.zero)
        {
            Gizmos.DrawLine(_originalPos, _originalPos + otherPos);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + otherPos);
        }
        
    }
}
