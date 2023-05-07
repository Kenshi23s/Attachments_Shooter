using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FacundoColomboMethods;
using System.Linq;

public class IA_Manager : MonoSingleton<IA_Manager>
{
    [SerializeField]
    LayerMask _obstacle, _walls;
    public LayerMask obstacle => _obstacle;
    public LayerMask wall_Mask => _walls;

    public List<IA_Movement> flockingTargets => _flockingTargets; 
    private List<IA_Movement> _flockingTargets;

    [SerializeField] bool debugNodeConnections;

    public List<Node> nodes;

    public void AddToFlockingTargets(IA_Movement a) => _flockingTargets.Add(a);

    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();
        nodes = transform.GetChildrenComponents<Node>().ToList();
        nodes.ForEach(x =>
        {
            x.IntializeNode();
            x.GetComponent<DebugableObject>().canDebug = debugNodeConnections;

        });
    }

    public Node GetNearestNode(Vector3 pos)
    {
        return ColomboMethods.GetNearestOnSigth(pos, nodes, wall_Mask);
    }
}
