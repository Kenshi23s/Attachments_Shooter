using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class sc_guidDesingManager : MonoBehaviour
{
    public sc_guid prefab;
    public float distancePerGuide;
    public Color defaultColor;

    public void CreateGuide(float distance)
    {
        sc_guid myGuid = Instantiate(prefab,transform.position+Vector3.up*2,Quaternion.identity).GetComponent<sc_guid>();
        myGuid.myColor = defaultColor;
        myGuid.transform.parent = this.transform;
        myGuid.myManager = this;
        myGuid.GetNeighbors(distance);
    }

    public void UpdateGuides(float distance)
    {
        this.GetComponentsInChildren<sc_guid>().ToList().ForEach(x => x.GetNeighbors(distance));
    }
}
