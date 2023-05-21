using System.Collections.Generic;
using UnityEngine;
using FacundoColomboMethods;
using System.Linq;
using System;

[RequireComponent(typeof(DebugableObject))]
public class Node : MonoBehaviour
{


    public List<Node> Neighbors = new List<Node>();
    [SerializeField] public int cost = 0;


    public void AddCost(int value) =>  cost += value * 1; 
    public void SubstractCost(int value) => cost = Mathf.Clamp(cost-(value * 1),0,int.MaxValue); 

   
    public void IntializeNode()
    {
   
        LayerMask wallMask = IA_Manager.instance.wall_Mask;

        Neighbors = IA_Manager.instance.nodes.GetWhichAreOnSight(transform.position, wallMask, RaycastType.Sphere, 2f)
                    .Where(x=> x!=this).ToList();

        GetComponent<DebugableObject>().AddGizmoAction(NodeGizmo);
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void NodeGizmo()
    {
       if (Neighbors.Count < 0) return;

        List<Node> nodes2 = Neighbors.Aggregate(new List<Node>(), (x, y) =>
        {
            if (y.Neighbors.Contains(this)) x.Add(y);
            return x;
        });
       foreach (Node node in nodes2)
       {      
         Gizmos.color = Color.blue;
         Gizmos.DrawLine(node.transform.position, transform.position);       
       }
            
          
        
           
        //Gizmos.DrawWireSphere(transform.position, AgentsManager.instance.nodeInteractradius);
    }
  
}
