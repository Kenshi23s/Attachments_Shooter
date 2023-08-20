using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class TerrainBlendingBaker : MonoBehaviour
{
    public RenderTexture depthTexture;
    public RenderTexture normalTexture;
    public RenderTexture albedoTexture;

    public ScriptableRendererFeature depthFeature;
    public ScriptableRendererFeature normalFeature;
    public ScriptableRendererFeature unlitTerrainFeature;

    // La camara atada a este script
    private Camera cam;

    [ContextMenu("Bake All")]
    public void BakeAll() 
    {
        BakeTerrainDepth();
        BakeTerrainAlbedo();
        BakeTerrainNormals();
    }

    [ContextMenu("Bake Depth Texture")]
    public void BakeTerrainDepth()
    {
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
    }

    [ContextMenu("Bake Normal Texture")]
    public void BakeTerrainNormals()
    {
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
    }


    [ContextMenu("Bake Albedo Texture")]
    public void BakeTerrainAlbedo()
    {
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