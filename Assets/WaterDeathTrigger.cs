using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDeathTrigger : MonoBehaviour
{
    Player_Handler player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player_Handler auxplayer))
        {
            player = auxplayer;
            enabled = true;
        }
    }

    private void Update()
    {
        if (!player) return; 
        if (player.transform.position.y<transform.position.y)
        {
            player.myHealth.TakeDamage(1000000);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player_Handler auxplayer))
        {
            player = null;
            enabled = false;
        }
    }
}
