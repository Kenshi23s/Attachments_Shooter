using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class Player_Handler : MonoBehaviour
{
    //public GunHandler GunHandler { get; private set;}
    public LifeComponent Health { get; private set;}

    public static Transform Transform;
    public static Vector3 Position,Velocity;

    [field: SerializeField] public RectTransform AbilitiesPanel { get; private set; }

    [field: SerializeField] public PlayerMovement Movement { get; private set; }

    [field: SerializeField] public GrabableHandler GrabHandler { get; private set; }

    [SerializeField,Range(0,180)] public float InteractFov;

    [SerializeField] sc_vibrateCamera vibrate;

    UnityEvent OnUpdate, OnLateUpdate, OnFixedUpdate = new UnityEvent();

    UnityEvent<Collision> WhenCollisionEnter, WhenCollisionStay, WhenCollisionExit = new UnityEvent<Collision>();

    public Action<IGrabable> GrabCallback;

    private void Awake()
    {
        //GunHandler = GetComponentInChildren<GunHandler>(); GunHandler.SetPlayer(this);

        Movement = GetComponent<PlayerMovement>();
        GrabHandler = GetComponent<GrabableHandler>();
        Health = GetComponent<LifeComponent>();
        Health.OnTakeDamage.AddListener((x) => vibrate.AddIntensity(x));

        Transform = transform;
       
    }

    private void Start()
    {
        Health.OnTakeDamage.RemoveListener(Health.ShowDamageNumber);
    }

    private void Update()
    {
        InteractablesManager.instance.UpdateInteractions(this);
        Position = transform.position;
        Velocity = Movement.Velocity;
       
    }
}
