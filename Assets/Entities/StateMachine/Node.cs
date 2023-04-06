using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
   
    public List<Node> Neighbors=new List<Node>();
    public float cost=0;
    [SerializeField]
    bool used;
    public bool agentNear=>used;
    public Vector3 position => transform.position;
    public void ChangeNear(bool value) => used = value;

    private void OnDrawGizmos()
    {           
         Gizmos.color = Color.blue;

         foreach (Node node in Neighbors)
         {
             foreach (Node neighbor in node.Neighbors)
             {
                 if (neighbor == this)
                 {
                     Gizmos.DrawLine(node.transform.position, transform.position);
                 }

             }
         
         }
        
        if (used==true)
        {
            Gizmos.color = Color.red;
          

        }
        else
        {
            Gizmos.color = Color.yellow;

        }
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
