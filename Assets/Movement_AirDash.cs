using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement_AirDash : MonoBehaviour
{

    public UnityEvent OnDash, OnDashReady = new UnityEvent();
    public PlayerMovement MovementOwner { get; private set; }
    public KeyCode LastKeyPresed { get; private set; }

    [SerializeField] float dashImpulse = 2f;
    [SerializeField] float dashCooldown;
    [SerializeField] bool ExecuteDash, DesiredDash;

  

    [field: SerializeField] public bool DashAvailable { get; private set; } = true;
    public void Awake()
    {
        MovementOwner = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (MovementOwner == null) { Destroy(this); return; }
      
      

    }


    #region AirDash Logic

    void LastMovementKey()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
            LastKeyPresed = KeyCode.W;

        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            LastKeyPresed = KeyCode.A;

        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            LastKeyPresed = KeyCode.D;

        }
        else
        {
            LastKeyPresed = KeyCode.S;
            return;
        }




    }


    private void Update()
    {
        LastMovementKey();
        DesiredDash = Input.GetAxis("Mouse ScrollWheel") < 0f;
    }

    private void FixedUpdate()
    {
        ExecuteDash = !MovementOwner.OnGround && DesiredDash && DashAvailable;
        if (ExecuteDash) AirDash();

    }

    IEnumerator DashCD()
    {
        DashAvailable = false;
        yield return new WaitUntil(() => MovementOwner.OnGround);
        DashAvailable = true; OnDashReady?.Invoke();

    }

    void AirDash()
    {
        DesiredDash = false;
        MovementOwner.ClearForces();
        //lo tengo para debugear por si no entra a ninguna(no deberia pasar)
        Vector3 DashDir = Vector3.up * 100;
        Debug.Log("Dash");
        LastMovementKey();
        switch (LastKeyPresed)
        {
            case KeyCode.W:
                DashDir = Vector3.forward;
                break;

            case KeyCode.D:
                DashDir = Vector3.right;
                break;

            case KeyCode.A:
                DashDir = Vector3.left;
                break;

            default:
                DashDir = Vector3.back;
                break;

        }

        DashDir = DashDir.GetOrientedVector(transform);

        MovementOwner.RigidBody.AddForce(DashDir.normalized * dashImpulse, ForceMode.Impulse);
        OnDash?.Invoke();
        StartCoroutine(DashCD());
    }
    #endregion
}
