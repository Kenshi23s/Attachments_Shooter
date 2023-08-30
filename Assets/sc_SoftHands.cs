using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SoftHands : MonoBehaviour
{

    public PlayerMovement myp;
    Vector3 mov;
    Vector3 initialPos;
    float rot;

    public float limitRot;
    public float limitFall;

    public float intensityRot;
    public float intensityMov;
    public float intensityFall;

    public float Softness;
    public float SoftnessRot;

    private void Start()
    {
        initialPos = transform.localPosition;
    }

    private void Update()
    {
        GetVars();
        RelocateArms();
    }

    public void GetVars()
    {
        mov = myp.RB.velocity;
        rot = Mathf.Clamp(myp.lookHorizontalSensitivity * Input.GetAxisRaw("Mouse X"),-limitRot,limitRot);
    }

    public void RelocateArms()
    {
        Vector3 velo = Vector3.Scale(myp.transform.right, mov);
        transform.localRotation = Quaternion.Lerp( transform.localRotation, Quaternion.Euler( 0, rot * intensityRot, ((velo.x + velo.z) * -0.5f) * intensityMov), SoftnessRot);
        transform.localPosition = Vector3.Lerp(initialPos, new Vector3(0, - Mathf.Clamp(mov.y, -limitFall, limitFall) * intensityFall, 0) + initialPos, Softness);
    }

}
