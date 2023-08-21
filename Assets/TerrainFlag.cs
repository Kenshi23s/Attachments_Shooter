using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFlag : MonoBehaviour
{
    public Terrain Terrain { get; private set; }
    public Renderer rend { get; private set; }
    Collider terrainCollider;
    private void Awake()
    {
        if (!TryGetComponent<Terrain>(out var l_terrain)) Destroy(this);
        Terrain = l_terrain;
        rend = Terrain.GetComponent<Renderer>();
        terrainCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        TerrainBlendingBaker.instance.AddTerrain(this);
        enabled = false;
    }

    public bool OnBakingCamera(Plane[] frustumPlanes)
    {
        //si el collider esta entre los 4 planos de la camara, devuelve verdadero
        return GeometryUtility.TestPlanesAABB(frustumPlanes, terrainCollider.bounds);
    }
}
