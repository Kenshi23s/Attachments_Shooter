using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Entity
{

    public static Vector3 _velocity;

    Rigidbody rb;

    public Transform MainCam;
    public Transform Player;
    CapsuleCollider mycollider;
    public LayerMask mycolision;

    public float sens = 2;
    float ejeY = 0;
    float ejeX = 0;
    float Xrotation = 0;

    public bool onGrounded;
    public bool onRunning;
    public bool onCrouch;

    public float groundDistance;
    public float friction;
    public float speed;
    public float speedJump;
    public float maxvelocity;

    public float heightCrouch;
    public float heightStand;


    public float maxRunningVel;
    public float maxWalkingVel;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();

        mycollider = transform.GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 140;

        Physics.gravity = new Vector3(0, -20f, 0);

        heightStand = mycollider.height;
    }

    void Update()
    {
        //detecta si el jugador corre (si el personaje no avanza hacia adelante no puede correr)
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && !onCrouch)
        {
            onRunning = true;
            maxvelocity = maxRunningVel;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            onRunning = false;
            maxvelocity = maxWalkingVel;
        }

        RotateCamera("Mouse X", "Mouse Y");

        //movimiento de teclas wasd
        if (onGrounded)
        {
            MovementKey(KeyCode.A, -Player.right, speed);
            MovementKey(KeyCode.W, Player.forward, speed);
            MovementKey(KeyCode.S, -Player.forward, speed);
            MovementKey(KeyCode.D, Player.right, speed);
        }
        DetectOnGrounded();

        //salto basico
        if (Input.GetKeyDown(KeyCode.Space) && onGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, speedJump, rb.velocity.z);            
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            mycollider.height = heightCrouch;
            onCrouch = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            mycollider.height = heightStand;
            onCrouch = false;
        }

        // devuelvo a la velocidad que voy a una variable estatica
        // (para que las balas no colisionen conmigo sumo su velocidad con la mia)
        _velocity = rb.velocity;
    }
    

    public void MovementKey(KeyCode mykey, Vector3 newMovement, float force)
    {
        RaycastHit myhit;

        if (Input.GetKey(mykey))
        {
            rb.velocity += newMovement.normalized * force * Time.deltaTime;
        }
    }

    public void DetectOnGrounded()
    {
        RaycastHit myhit;

        if (Physics.SphereCast(transform.position + (Vector3.down * ((mycollider.height / 2) - mycollider.radius * 2) * transform.localScale.y),
                                mycollider.radius * transform.localScale.y, -Player.up, out myhit, groundDistance, mycolision)
                               && !myhit.collider.isTrigger)
        {
            onGrounded = true;
            
            Friction();
        }
        else
        {
            onGrounded = false;
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

    public void Friction()
    {
        Vector3 tempDir = rb.velocity.normalized;
        rb.velocity -= rb.velocity.normalized * friction * Time.deltaTime;

        if(rb.velocity.normalized * -1 == tempDir)
        {
            rb.velocity = Vector3.zero;
        }
        LimitVelocity();
    }

    public void SetVelocity(Vector3 dir, float force)
    {
        rb.velocity = dir.normalized * force;
    }

    public void RotateCamera(string axisX, string axisY)
    {
        
            ejeX = Input.GetAxisRaw(axisX) * sens;
            ejeY = Input.GetAxisRaw(axisY) * sens;

            Xrotation -= ejeY;
            Xrotation = Mathf.Clamp(Xrotation, -90, 90);

            Player.Rotate(Vector3.up * ejeX);
            MainCam.transform.localRotation = Quaternion.Euler(Xrotation, 0, 0);

    }

    #region UtilidadesEntidad

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    public override void Resume()
    {
        throw new System.NotImplementedException();
    }

    protected override int OnTakeDamage(int dmgDealt)
    {
        return dmgDealt / 10;
    }

    public override bool WasCrit()
    {
        return false;
    }

    public override bool WasKilled()
    {
        return false;
    }

    public override void OnDeath()
    {
        //Debug.LogError("moriste, pero como se esta testeando no pasa nada");
    }
    #endregion
}


