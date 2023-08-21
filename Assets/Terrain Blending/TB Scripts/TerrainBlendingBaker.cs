using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Rendering.Universal;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class TerrainBlendingBaker : MonoSingleton<TerrainBlendingBaker>
{
    public RenderTexture depthTexture;
    public RenderTexture normalTexture;
    public RenderTexture albedoTexture;

    public ScriptableRendererFeature depthFeature;
    public ScriptableRendererFeature normalFeature;
    public ScriptableRendererFeature unlitTerrainFeature;

    // La camara atada a este script
    private Camera cam;
    [SerializeField]Transform FollowTarget;
    public float UnitsBeforReBaking = 100;

    Terrain terrain;

    List<TerrainFlag> terrainList = new List<TerrainFlag>();

    public void AddTerrain(TerrainFlag x)
    {
        terrainList.Add(x);
    }

    protected override void SingletonAwake()
    {
        cam = GetComponent<Camera>();
    }
    private void Start()
    {
        BakeEveryXUnits();
        StartCoroutine(BakeEveryXUnits());
    }



    IEnumerator BakeEveryXUnits()
    {
        yield return null;
        while (FollowTarget != null)
        {
            Vector3 CamPos = FollowTarget.position;
            CamPos.y = transform.position.y;
            transform.position = CamPos;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

            FList<Terrain> onCamera = new FList<Terrain>();
            foreach (var item in terrainList)
            {
                if (item.OnBakingCamera(planes))                      
                    onCamera += item.Terrain;                        
            }

            BakeTerrainDepth(onCamera);
            yield return null; BakeTerrainNormals(onCamera);
            yield return null; BakeTerrainAlbedo(onCamera);
        
   
            yield return new WaitUntil(() => Vector3.Distance(transform.position, FollowTarget.position) > UnitsBeforReBaking);
        }
    }



    public void OffTrees(IEnumerable<Terrain> collection)
    {
        foreach (var item in collection)
        {
            item.drawTreesAndFoliage = false;
        }
    }

    public void OnTrees(IEnumerable<Terrain> collection)
    {
        foreach (var item in collection)
        {
            item.drawTreesAndFoliage = true;
        }
    }
    [ContextMenu("Bake All")]
    public void BakeAll()
    {
        terrain = GetComponentInParent<Terrain>();
       
        BakeTerrainDepth(FList.Create(terrain));
        BakeTerrainAlbedo(FList.Create(terrain));
        BakeTerrainNormals(FList.Create(terrain));
      
    }

    [ContextMenu("Bake Depth Texture")]
    public void BakeTerrainDepth(IEnumerable<Terrain> col)
    {
        OffTrees(col);
        unlitTerrainFeature.SetActive(false);
        normalFeature.SetActive(false);
        depthFeature.SetActive(true);

        UpdateBakingCamera();

        if (!depthTexture)
        {
            Debug.LogWarning("[Custom Msg] Se debe asignar la depth texture en el inspector.");
            return;
        }

        cam.targetTexture = depthTexture;
        Shader.SetGlobalTexture("TB_DEPTH", depthTexture);

        cam.Render();
        OnTrees(col);
    }

    [ContextMenu("Bake Normal Texture")]
    public void BakeTerrainNormals(IEnumerable<Terrain> col)
    {
        OffTrees(col);
        depthFeature.SetActive(false);
        unlitTerrainFeature.SetActive(false);
        normalFeature.SetActive(true);

        UpdateBakingCamera();

        if (!normalTexture)
        {
            Debug.LogWarning("[Custom Msg] Se debe asignar la normal texture en el inspector.");
            return;
        }

        cam.targetTexture = normalTexture;
        Shader.SetGlobalTexture("TB_NORMAL", normalTexture);

        cam.Render();
        OnTrees(col);
    }


    [ContextMenu("Bake Albedo Texture")]
    public void BakeTerrainAlbedo(IEnumerable<Terrain> col)
    {
        OffTrees(col);
        depthFeature.SetActive(false);
        normalFeature.SetActive(false);
        unlitTerrainFeature.SetActive(true);

        UpdateBakingCamera();

        if (!albedoTexture)
        {
            Debug.LogWarning("[Custom Msg] Se debe asignar la albedo texture en el inspector.");
            return;
        }

        cam.targetTexture = albedoTexture;
        Shader.SetGlobalTexture("TB_ALBEDO", albedoTexture);

        cam.Render();
        OnTrees(col);
    }

    private void UpdateBakingCamera()
    {
        // Si no se asigno una camara, asignarsela.
        if (!cam && !TryGetComponent(out cam))
        {
            Debug.LogWarning("[Custom Msg] No hay ninguna camara acoplada a este GameObject");
            return;
        }

        // El ancho total de la proyeccion de nuestra camara
        Shader.SetGlobalFloat("TB_SCALE", cam.orthographicSize * 2);
        // La esquina inferior de la textura en escala mundial
        Shader.SetGlobalFloat("TB_OFFSET_X", cam.transform.position.x - cam.orthographicSize);
        Shader.SetGlobalFloat("TB_OFFSET_Z", cam.transform.position.z - cam.orthographicSize);
        // La posicion en 'y' relativa de la camara. 
        Shader.SetGlobalFloat("TB_OFFSET_Y", cam.transform.position.y - cam.farClipPlane);
        // El plano de recorte lejano para saber el rango de valores en 'y' en la textura de profundidad
        Shader.SetGlobalFloat("TB_FARCLIP", cam.farClipPlane);
    }


}