using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Camera MainCam;
    public Transform Player;

    public float sens = 2;
    public float ejeY = 0;
    public float ejeX = 0;

    public float Xrotation = 0;
    public float Yrotation = 0;

    public float speed;
    public float speedJump;
    public float maxvelocity;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
         rb = transform.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 140;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            ejeX = Input.GetAxis("Mouse X") * sens * Time.deltaTime * 100;
            ejeY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime * 100;

            Xrotation -= ejeY;
            Xrotation = Mathf.Clamp(Xrotation,-90,90);

            MainCam.transform.localRotation = Quaternion.Euler(Xrotation,0,0);
            Player.Rotate(Vector3.up * ejeX);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += Player.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity -= Player.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity -= Player.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity += Player.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, speedJump, rb.velocity.z);
        }

        Vector3 HorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (HorizontalVelocity.magnitude > maxvelocity)
        {
            HorizontalVelocity = HorizontalVelocity.normalized * maxvelocity;
            rb.velocity = new Vector3(HorizontalVelocity.x, rb.velocity.y, HorizontalVelocity.z);
        }
    }
}
