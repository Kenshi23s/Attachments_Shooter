using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    [Header("Caminar"), Space(1)]
    public float _walkFrequency = 0.7f;
    public float _walkAmplitude = 1f;
    public float _minWalk = 1f;

    [Header("Correr"), Space(1)]
    public float _runFrequency = 1f;
    public float _runAmplitude = 1f;
    public float _minRun = 1f;

    [Header("Suavizar Animacion"), Space(1)]
    [Range(0f, 1f)]
    public float soft = 0.3f;
    [Range(0f, 1f)]
    public float handsInfluence = 0.5f;

    [Header("Apuntado"), Space(1)]
    public Vector3 aimPos;
    bool aiming;
    Vector3 actualAimPos;

    [Header("Referencias"), Space(1)]
    public Transform hands;
    public Camera cam;
    [SerializeField] Player_Movement playerMov;
    [SerializeField] Rigidbody rb;

    public void Update()
    {
        Vector3 temp1;
        Vector3 temp2;

        if (rb.velocity.magnitude > _minRun && playerMov.OnGrounded)
        {
            temp1 = Vector3.Lerp(cam.transform.localPosition, FootStepMotion(_runFrequency, _runAmplitude), soft);
            temp2 = Vector3.Lerp(hands.transform.localPosition, FootStepMotion(_runFrequency, _runAmplitude) * handsInfluence, soft);
        }
        else
        {
            if (rb.velocity.magnitude > _minWalk && playerMov.OnGrounded)
            {
                temp1 = Vector3.Lerp(cam.transform.localPosition, FootStepMotion(_walkFrequency, _walkAmplitude), soft);
                temp2 = Vector3.Lerp(hands.transform.localPosition, FootStepMotion(_walkFrequency, _walkAmplitude) * handsInfluence, soft);
            }
            else
            {
                temp1 = Vector3.Lerp(cam.transform.localPosition, Vector3.zero, soft);
                temp2 = Vector3.Lerp(hands.transform.localPosition, Vector3.zero, soft);
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            actualAimPos = Vector3.Lerp(actualAimPos,aimPos,soft);

            hands.transform.localPosition = actualAimPos + temp1;
        } 
        else
        {
            hands.transform.localPosition = temp2;
            actualAimPos = Vector3.zero;
        }
        cam.transform.localPosition = temp1;
        



    }


    public Vector3 FootStepMotion(float frequency, float amplitude)
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude*1.2f;
        pos.x += Mathf.Cos(Time.time * frequency/2) * amplitude * 2;
        return pos;
    }
}
