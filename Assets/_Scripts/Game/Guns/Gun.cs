using UnityEngine;
using System;
using System.Collections.Generic;
using static StatsHandler;
using AYellowpaper.SerializedCollections;
using System.Diagnostics;
using System.Collections;

//struct encargado de recolectar datos de los "Hits" y luego pasarlo por un evento a los interesados
public struct HitData
{
    public Vector3 _impactPos;
    public DamageData dmgData;
    public Gun gunUsed;

    public HitData(Vector3 pos, DamageData dmgData, Gun weapon)
    {
        _impactPos = pos;
        this.dmgData = dmgData;      
        this.gunUsed = weapon;
    }
}

#region Components
[RequireComponent(typeof(DamageHandler))]
[RequireComponent(typeof(StatsHandler))]
[RequireComponent(typeof(AttachmentHandler))]
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(PerkHandler))]
#endregion
[RequireComponent(typeof(PausableObject))]
public abstract class Gun : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator _gunAnim;

    [SerializeField] public int _actualAmmo;
    #region Components
    [NonSerialized] public DamageHandler damageHandler;

    [NonSerialized] public StatsHandler stats;

    [NonSerialized] public AttachmentHandler attachmentHandler;

    [NonSerialized] public PerkHandler perkHandler;

    [NonSerialized] public DebugableObject _debug;

   protected PausableObject _pausableObject;

    #endregion

    #region Events
    //estos eventos se usarian para callback hacia los perks y para feedback(particulas, sonidos,animaciones) 

    public event Action everyTick;

    public event Action onReload;

    public event Action onStow;

    public event Action onTriggerPressed;

    public event Action onTriggerReleased;

    public event Action onDraw;

    public event Action onShoot;

    public event Action<HitData> onHit;

    //en vez de tener un evento para cada una de estas condiciones, paso toda la info en un struct(hitData),
    //lo malo de esto es que va a haber llamados a los cual no les importe algo de lo q hay guardado en el struct,
    //por ej si le paso a la pool de particulas donde fue el impacto, no le va a importar si fue critico o no(o quizas si?)


    #endregion

    MeshRenderer _meshRenderer;

    bool _triggerPressed;
    public bool TriggerPressed
    {
        get => _triggerPressed;

        protected set
        {
            _triggerPressed = value;

            if (_triggerPressed)
                onTriggerPressed?.Invoke();
            else
                onTriggerReleased?.Invoke();
        }

    }

    public bool canShoot { get; protected set; }

    public abstract void Shoot();
    public abstract bool ShootCondition();

    public void OnHitCallBack(HitData data) => onHit?.Invoke(data);

    protected virtual void Awake()
    {
        canShoot = true;
        _triggerPressed = false;

        attachmentHandler = GetComponent<AttachmentHandler>();
        damageHandler = GetComponent<DamageHandler>();
        perkHandler = GetComponent<PerkHandler>();
        stats = GetComponent<StatsHandler>();
        _debug = GetComponent<DebugableObject>();
        _pausableObject = GetComponent<PausableObject>();
        _pausableObject.onPause += () => StartCoroutine(RememberTrigger());

        _meshRenderer ??= GetComponentInChildren<MeshRenderer>();

        OptionalInitialize();
    }

    protected virtual void Start() 
    {
        SetInventoryEvents();
    }

    protected virtual void OptionalInitialize() { }

    public void GunUpdate() => everyTick?.Invoke();

    //lo ideal seria tener un metodo q llame a la animacion de recarga,
    //y que en algun frame la animacion de recarga llame a este metodo
    public void Reload()
    {
        //_actualAmmo = (int)stats.statDictionary[StatNames.MaxMagazine];
        onReload?.Invoke();
    }

    /// <summary>
    /// Intenta disparar el arma, las herencias por lo general modifican por completo esto
    /// </summary>
    public virtual void PressTrigger()
    {
        //talvez deberia tener un metodo abstracto para las condiciones de disparo,
        //y cada arma eligiria cual quiere que sean sus condiciones para dispararse

        TriggerPressed = true;
        everyTick += WhileTriggerPressed;
    }

    IEnumerator RememberTrigger()
    {
        bool aux = TriggerPressed;
        TriggerPressed = false;
        everyTick -= WhileTriggerPressed;

        yield return new WaitWhile(ScreenManager.IsPaused);  
        
        if (aux) PressTrigger();
    }

    void WhileTriggerPressed()
    {
        if (ShootCondition())
        {
            Shoot();
            onShoot?.Invoke();
        }
    }

    public void ReleaseTrigger()
    {
        TriggerPressed = false;
        everyTick -= WhileTriggerPressed;
    }

    protected void CallOnShootEvent() => onShoot?.Invoke();

    public void Draw()
    {
        onDraw?.Invoke();
    }

    public void Stow()
    {
        onStow?.Invoke();
    }

    void SetInventoryEvents()
    {
        AttachmentManager.instance.OnInventoryClose += StopCastingShadows;
        AttachmentManager.instance.OnInventoryOpen += StartCastingShadows;
    }

    public void StopCastingShadows()
    {
        _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void StartCastingShadows()
    {
        _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
}
