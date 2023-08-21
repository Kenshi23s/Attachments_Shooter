using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class Player_Handler : MonoBehaviour
{
    public GunHandler myGunHandler { get; private set;}
    public LifeComponent myHealth { get; private set;}

    [field: SerializeField] public PlayerMovement Movement { get; private set; }

    [SerializeField,Range(0,180)] public float InteractFov;

    [SerializeField] sc_vibrateCamera vibrate;

    UnityEvent OnStart, OnUpdate, OnLateUpdate, OnFixedUpdate = new UnityEvent();

    UnityEvent<Collision> WhenCollisionEnter, WhenCollisionStay, WhenCollisionExit = new UnityEvent<Collision>();

    private void Awake()
    {
        myGunHandler = GetComponentInChildren<GunHandler>();
        myGunHandler.SetPlayer(this);
        Movement = GetComponent<PlayerMovement>();

        myHealth = GetComponent<LifeComponent>();
        myHealth.OnTakeDamage.AddListener((x) => vibrate.AddIntensity(x));
    }

    private void Start()
    {
        myHealth.OnTakeDamage.RemoveListener(myHealth.ShowDamageNumber);
    }

    private void Update()
    {
        InteractablesManager.instance.UpdateInteractions(this);
    }
}
