using UnityEngine;
using UnityEngine.Events;

public class Player_Handler : MonoBehaviour
{
    public GunHandler myGunHandler { get; private set;}
    public LifeComponent myHealth { get; private set;}

    [field: SerializeField] public PlayerMovement Movement { get; private set; }

    [SerializeField,Range(0,180)] public float InteractFov;

    [SerializeField] sc_vibrateCamera vibrate;

    UnityEvent OnStart, OnUpdate, OnLateUpdate, OnFixedUpdate = new UnityEvent();

    UnityEvent<Collision> WhenCollisionEnter, WhenCollisionStay, WhenCollisionExit = new UnityEvent<Collision>();

    private void OnValidate()
    {
        Movement.Validate();
    }

    private void Awake()
    {
        myGunHandler = GetComponentInChildren<GunHandler>();
        myGunHandler.SetPlayer(this);


        myHealth = GetComponent<LifeComponent>();
        myHealth.OnTakeDamage.AddListener((x) => vibrate.AddIntensity(x));

        Movement.Configure(this);
        Movement.Validate();
    }

    private void Start()
    {
        myHealth.OnTakeDamage.RemoveListener(myHealth.ShowDamageNumber);
    }

    private void Update()
    {
        InteractablesManager.instance.UpdateInteractions(this);
        Movement.Update();
    }

    private void FixedUpdate()
    {
        Movement.FixedUpdate();
    }

    private void LateUpdate()
    {
        Movement.LateUpdate();
    }

    private void OnCollisionStay(Collision collision)
    {
        Movement.EvaluateCollision(collision);
    }
}
