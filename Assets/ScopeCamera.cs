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

    [SerializeField]
    float _additionalFOV = 5f;

    [SerializeField]
    Renderer _scopeRenderer;
    Material _scopeMaterial;
    [SerializeField] RenderTexture myRenderTexture;

    [Header("Aim Texture Resolution")]
    [SerializeField] int textureAimWidth;
    [SerializeField] int textureAimHeight;
    [Header("HipFire Texture Resolution")]
    [SerializeField] int HipfireWidth;
    [SerializeField] int HipfireHeight;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _owner = GetComponentInParent<Sight>();
        _scopeMaterial = _scopeRenderer.material;

        TurnOffScope();

        // Guardar valores originales
        _ogParent = transform.parent;
        _ogLocalPosition = transform.localPosition;
        _ogLocalRotation = transform.localRotation;

        _owner.onAttach += SetValuesToMainCamera;
        _owner.onDettach += ResetValues;
        
    }

    private void Start()
    {
        
    }


    void SetValuesToMainCamera() 
    {
        // Igualar a los valores de la camara principal
        transform.parent = Camera.main.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        ShakeCamera.OnAimTransition += UpdateCamera;
        ShakeCamera.AimStart += SetAimRenderTexture;
        ShakeCamera.AimEnd += SetHipFireRenderTexture;



    }
    void SetAimRenderTexture() => SetRenderTexture(textureAimWidth, textureAimHeight);

    void SetHipFireRenderTexture() => SetRenderTexture(HipfireWidth, HipfireHeight);

    void SetRenderTexture(int x,int y)
    {
        // "borro" la textura
        myRenderTexture.Release();
        //asigno valores
        myRenderTexture.width = x;
        myRenderTexture.height = y;
        //la vuelvo a crear
        myRenderTexture.Create();

    }
    void ResetValues() 
    {
        // Restaurar valores de la camara
        transform.parent = _ogParent;
        transform.localPosition = _ogLocalPosition;
        transform.localRotation = _ogLocalRotation;

        TurnOffScope(); SetHipFireRenderTexture();

        ShakeCamera.OnAimTransition -= UpdateCamera;
        ShakeCamera.AimStart -= SetAimRenderTexture;
        ShakeCamera.AimEnd -= SetHipFireRenderTexture;
    }

    // Actualizar los valores de la mira y la camara segun que tan cerca esta de llegar a su posicion final de apuntado
    void UpdateCamera(float t)
    {
        if (t > 0)
        {
            _camera.enabled = true;
            _camera.fieldOfView = Camera.main.fieldOfView + _additionalFOV;
            _scopeMaterial.SetFloat("_Fade", 1 - t);
          
        }
        else
        {
            TurnOffScope();
        }
    }

    void TurnOffScope() 
    {
        _camera.enabled = false;
        _scopeMaterial.SetFloat("_Fade", 1);
    }
}
