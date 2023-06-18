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
    }

    private void Start()
    {
        _owner.onAttach += SetValuesToMainCamera;
        _owner.onDettach += ResetValues;
    }


    void SetValuesToMainCamera() 
    {
        // Igualar a los valores de la camara principal
        transform.parent = Camera.main.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        ShakeCamera.OnAimTransition += UpdateCamera;
    }

    void ResetValues() 
    {
        // Restaurar valores de la camara
        transform.parent = _ogParent;
        transform.localPosition = _ogLocalPosition;
        transform.localRotation = _ogLocalRotation;

        TurnOffScope();
        ShakeCamera.OnAimTransition -= UpdateCamera;
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
