using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Transform MainCam;
    public Transform Player;
    CapsuleCollider mycollider;
    public LayerMask mycolision;

    public float sens = 2;
    float ejeY = 0;
    float ejeX = 0;
    float Xrotation = 0;

    public bool OnGrounded;
    public float groundDistance;
    public float speed;
    public float speedJump;
    public float maxvelocity;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        mycollider = transform.GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 140;

        Physics.gravity = new Vector3(0, -20f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            ejeX = Input.GetAxis("Mouse X") * sens;
            ejeY = Input.GetAxis("Mouse Y") * sens;

            Xrotation -= ejeY;
            Xrotation = Mathf.Clamp(Xrotation,-90,90);

            MainCam.transform.localRotation = Quaternion.Euler(Xrotation,0,0);
            Player.Rotate(Vector3.up * ejeX);
        }

        //movimiento de teclas wasd
        MovementKey(KeyCode.A, -Player.right, speed);
        MovementKey(KeyCode.W, Player.forward, speed);
        MovementKey(KeyCode.S, -Player.forward, speed);
        MovementKey(KeyCode.D, Player.right, speed);
        DetectOnGrounded();

        //salto basico
        if (Input.GetKeyDown(KeyCode.Space) && OnGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, speedJump, rb.velocity.z);
        }

        LimitVelocity();
        FixFriccionMove();
    }

    public void MovementKey(KeyCode mykey, Vector3 newMovement, float force)
    {
        RaycastHit myhit;

        if (Input.GetKey(mykey))
        {
            if (Physics.CapsuleCast(transform.position + Vector3.down * ((mycollider.height / 2) - mycollider.radius*2) * transform.localScale.y,
                                    transform.position + Vector3.up * ((mycollider.height / 2) - mycollider.radius) * transform.localScale.y,
                                    mycollider.radius * transform.localScale.y * 0.9f, newMovement.normalized, out myhit, force * Time.deltaTime, mycolision))
            {
                newMovement = Vector3.ProjectOnPlane(newMovement, myhit.normal);
                Debug.LogWarning("se colisiono");
            }

            rb.velocity += newMovement.normalized * force * Time.deltaTime;
        }
    }

    /// <summary>
    /// este sistema funciona para perdonar choques contra paredes o rampas (desliza sobre superficies en diagonal)
    /// </summary>
    public void FixFriccionMove()
    {

        RaycastHit myhit;

            if (Physics.CapsuleCast(
                transform.position + Vector3.down * ((mycollider.height / 2) - mycollider.radius * 2) * transform.localScale.y,
                transform.position + Vector3.up * ((mycollider.height / 2) - mycollider.radius) * transform.localScale.y,
                mycollider.radius * transform.localScale.y, rb.velocity.normalized * 0.9f, out myhit, rb.velocity.magnitude * Time.deltaTime, mycolision))
            {
                rb.velocity = Vector3.ProjectOnPlane(rb.velocity, myhit.normal);
                Debug.LogWarning("se colisiono");
            }
        
    }

    public void DetectOnGrounded()
    {
        RaycastHit myhit;

        if (Physics.SphereCast(transform.position + (Vector3.down * ((mycollider.height / 2) - mycollider.radius * 2) * transform.localScale.y),
                                mycollider.radius * transform.localScale.y, -Player.up, out myhit, groundDistance, mycolision))
        {
            Debug.Log("piso");
            OnGrounded = true;
        }
        else
        {
            OnGrounded = false;
        }       
    }

    public void LimitVelocity()
    {
        Vector3 HorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (HorizontalVelocity.magnitude > maxvelocity)
        {
            HorizontalVelocity = HorizontalVelocity.normalized * maxvelocity;
            rb.velocity = new Vector3(HorizontalVelocity.x, rb.velocity.y, HorizontalVelocity.z);
        }
    }

    public void SetVelocity()
    {

    }
}
