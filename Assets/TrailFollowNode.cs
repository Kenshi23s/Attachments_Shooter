using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollowNode : MonoBehaviour
{
    [SerializeField] List<GameObject> TrailNode = new List<GameObject>();
    [SerializeField] float distanceFollow = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TrailNode.Count > 1)
        {
            TravelTrail();
        }
    }

    public void TravelTrail()
    {
        for (int i = 0; i < TrailNode.Count - 1; i++)
        {

            FollowNext(TrailNode[i+1], TrailNode[i]);
        }
    }

    public void FollowNext(GameObject Node,GameObject NodeParent)
    {
        if ((NodeParent.transform.position - Node.transform.position).magnitude > distanceFollow)
        {
            Node.transform.position = NodeParent.transform.position + (Node.transform.position - NodeParent.transform.position).normalized * distanceFollow;
            Node.transform.LookAt(NodeParent.transform.position);

        }
    }
}
