using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Manager : MonoSingleton<IA_Manager>
{
    [SerializeField]
    LayerMask _obstacle, _walls;
    public LayerMask obstacle => _obstacle;
    public LayerMask wall_Mask => _walls;

    public List<Node> nodes;

    public List<IA_Movement> flockingTargets;


    public void AddToFlockingTargets(IA_Movement a) => flockingTargets.Add(a);
}
