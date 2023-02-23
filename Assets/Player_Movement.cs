using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Camera MainCam;

    public float sens = 2;
    public float ejeY = 0;
    public float ejeX = 0;

    public float speed;
    public float speedJump;
    public float maxvelocity;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
         rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            ejeX += Input.GetAxis("Mouse X") * sens;
            ejeY -= Input.GetAxis("Mouse Y") * sens;

            transform.eulerAngles = new Vector3(0, ejeX, 0);

            MainCam.transform.localEulerAngles = new Vector3(ejeY, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity -= transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity -= transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity += transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += transform.up * speedJump;
        }

        Vector3 HorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (HorizontalVelocity.magnitude > maxvelocity)
        {
            HorizontalVelocity = HorizontalVelocity.normalized * maxvelocity;
            rb.velocity = new Vector3(HorizontalVelocity.x, rb.velocity.y, HorizontalVelocity.z);
        }
    }
}
