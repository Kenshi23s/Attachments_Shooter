using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    public Transform spawnpos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out Player_Handler x))
        {
            x.transform.root.position = spawnpos.position;
            x.Rigidbody.velocity = Vector3.zero;
        }
    }
}
