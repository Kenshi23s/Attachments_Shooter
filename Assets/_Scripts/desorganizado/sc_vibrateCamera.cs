using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_vibrateCamera : MonoBehaviour
{
    //vibrar la camara
    Vector3 origin;
    Vector3 point;
    [SerializeField] float frequency = 0.01f;
    [SerializeField] float intensity = 0f;

    [Range(0f,1f)]
    [SerializeField] float magnitud = 0.025f;

    [Range(0f,100f)]
    [SerializeField] float stability = 4;

    [Range(0f, 10f)]
    [SerializeField] float soft;

    float Count;

    private void Awake()
    {
        origin = transform.localPosition;
        MovePoint();
    }
    

    private void Update()
    {

        //probable que se rompa
        transform.localPosition = Vector3.Lerp(transform.localPosition, point + origin, Mathf.Clamp(soft * Time.deltaTime));

        //Contar hasta cambiar el point
        Count += Time.deltaTime;

        if (Count >= frequency)
        {
            Count = 0;
            MovePoint();
        }

        //la intenstidad se reduce con el tiempo
        ReduceIntensity();
    }

    void MovePoint()
    {
        point = Random.insideUnitSphere.normalized * intensity * magnitud;
    }

    void ReduceIntensity()
    {
        intensity -= stability * Time.deltaTime;

        if (intensity < 0)
        {
            intensity = 0;
        }
    }

    #region metodos que se pueden llamar
    public void AddIntensity(float add)
    {
        intensity += add;
    }

    public void ChangeIntesity(float intens)
    {
        intensity = intens;
    }

    #endregion

}
