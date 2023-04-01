using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public float _frequency = 1f, _amplitude = 1f;
    public float _minAnim = 1f;
    public float soft = 0.3f;
    public Camera cam;
   [SerializeField] Rigidbody rb;

    public void Update()
    {
        if (rb.velocity.magnitude > _minAnim)
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, FootStepMotion(), soft);
        }
        else
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, Vector3.zero, soft);
        }
    }

    public Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude*1.2f;
        pos.x += Mathf.Cos(Time.time * _frequency/2) * _amplitude * 2;
        return pos;
    }
}
