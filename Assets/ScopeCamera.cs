using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeCamera : MonoBehaviour
{
    Sight _owner;
    Camera _camera;
    Transform _ogParent;
    Vector3 _ogLocalPosition;
    Quaternion _ogLocalRotation;

    Transform _mainCamera;

    [SerializeField]
    float _additionalFOV = 5f;

    [SerializeField]
    Renderer _scopeRenderer;
    Material _scopeMaterial;
    [SerializeField] RenderTexture myRenderTexture;

    [Header("Aim Texture Resolution")]
    [SerializeField] int textureAimWidth;
    [SerializeField] int textureAimHeight;

    private void Awake()
    {
        _mainCamera = Camera.main.transform;
        _camera = GetComponent<Camera>();
        _owner = GetComponentInParent<Sight>();
        _scopeMaterial = _scopeRenderer.material;

        TurnOffScope();

        // Guardar valores originales
        _ogParent = transform.parent;
        _ogLocalPosition = transform.localPosition;
        _ogLocalRotation = transform.localRotation;

        _owner.OnAttach += SetValuesToMainCamera;
        _owner.OnDettach += ResetValues;
        
    }


    void SetValuesToMainCamera() 
    {
        // Igualar a los valores de la camara principal
        transform.parent = _mainCamera;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        ShakeCamera.OnAimTransition += UpdateScope;
        ShakeCamera.OnHipPosReached += TurnOffScope;
        ShakeCamera.OnHipPosLeft += TurnOnScope;
    }

    void ResetValues() 
    {
        // Restaurar valores de la camara
        
        transform.parent = _ogParent;
        transform.localPosition = _ogLocalPosition;
        transform.localRotation = _ogLocalRotation;

        TurnOffScope(); 

        ShakeCamera.OnAimTransition -= UpdateScope;
        ShakeCamera.OnHipPosReached -= TurnOffScope;
        ShakeCamera.OnHipPosLeft -= TurnOnScope;
    }

    // Actualizar los valores de la mira y la camara segun que tan cerca esta de llegar a su posicion final de apuntado
    void UpdateScope(float t)
    {
        _camera.fieldOfView = Camera.main.fieldOfView - _additionalFOV;
        _scopeMaterial.SetFloat("_Fade", 1 - t);
    }

    void TurnOffScope() 
    {
        _camera.enabled = false;
        _scopeMaterial.SetFloat("_Fade", 1);
        ReleaseRenderTexture();
    }

    void TurnOnScope()
    {
        _camera.enabled = true;
        CreateRenderTexture(textureAimWidth, textureAimHeight);
    }

    void CreateRenderTexture(int x, int y)
    {
        // "Borro" la textura
        myRenderTexture.Release();
        // Asigno valores
        myRenderTexture.width = x;
        myRenderTexture.height = y;
        // La vuelvo a crear
        myRenderTexture.Create();

    }

    void ReleaseRenderTexture()
    {
        myRenderTexture.Release();
    }
}
