using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(IA_Movement))]
public class AgentTest : MonoBehaviour
{
    IA_Movement agent;
    [SerializeField]GameObject objectToLookFor;

    private void Awake()
    {
        agent = GetComponent<IA_Movement>();
    }
    private void Start()
    {
        Debug.Log(objectToLookFor);
        if (Vector3.Distance(objectToLookFor.transform.position, transform.position) > 2)
        {
            agent.SetDestination(objectToLookFor.transform.position);
        }
    }
   
}
