using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeCamera : MonoBehaviour
{
    Sight _owner;
    Transform _ogParent;
    Vector3 _ogLocalPosition;
    Quaternion _ogLocalRotation;

    private void Awake()
    {
        _owner = GetComponentInParent<Sight>();

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
    }

    void ResetValues() 
    {
        // Restaurar valores de la camara
        transform.parent = _ogParent;
        transform.localPosition = _ogLocalPosition;
        transform.localRotation = _ogLocalRotation;
    }
}
