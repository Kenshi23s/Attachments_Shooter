using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(IA_Movement))]
public class AgentTest : MonoBehaviour
{
    IA_Movement agent;
    GameObject objectToLookFor;

    private void Awake()
    {
        agent = GetComponent<IA_Movement>();
    }
    private void Update()
    {
        if (Vector3.Distance(objectToLookFor.transform.position,transform.position) < 2)
        {
            
        }
    }
}
