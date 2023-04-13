using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepObjectsInside : MonoBehaviour
{
    [System.NonSerialized]
    public Vector3 velocity;

    private void OnTriggerEnter(Collider other)
    {
        var myentity = other.GetComponent<Rigidbody>();

        if (myentity != null)
        {
            myentity.transform.parent = this.transform;
            myentity.velocity -= velocity / Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var myentity = other.GetComponent<Rigidbody>();

        if (myentity != null)
        {
            myentity.transform.parent = null;
            myentity.velocity += velocity / Time.deltaTime;
        }
    }
}
