using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class sc_guid : MonoBehaviour
{
    public sc_guidDesingManager myManager;
    public Color myColor;
    public List<sc_guid> neighbors;


    //private void Awake()
    //{
    //    Destroy(gameObject);

    //}
    public void GetNeighbors(float distance)
    {
        neighbors = myManager.GetComponentsInChildren<sc_guid>().Where(x => x != this).Where(x => (x.transform.position - transform.position).magnitude < distance).ToList();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = myColor;

        foreach (var item in neighbors)
        {
            if (item != null)
            {
                Gizmos.DrawLine(transform.position, Vector3.Lerp(item.transform.position, transform.position, 0.5f));
            }         
        }
    }
}
