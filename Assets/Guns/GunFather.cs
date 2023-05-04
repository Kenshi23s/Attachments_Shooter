using UnityEngine;
using System;
using System.Collections.Generic;
using static StatsHandler;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public struct GunAudioClips
{
    [Header("SFX")]
    //sonido inicial
    [SerializeField] public AudioClip _StartShootingClip;
    //sonido loop
    [SerializeField] public AudioClip _LoopShootingClip;
    //sonido final
    [SerializeField] public AudioClip _EndShootingClip;

    [SerializeField] public AudioClip _ReloadClip;
}
//struct encargado de recolectar datos de los "Hits"
public struct HitData
{
    public Vector3 _impactPos;
    public DamageData dmgData;
    public GunFather weapon;

    public HitData(Vector3 pos, DamageData dmgData, GunFather weapon)
    {
        _impactPos = pos;
        this.dmgData = dmgData;      
        this.weapon = weapon;
    }
}

public abstract class GunFather : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator _gunAnim;


    [SerializeField] public int _actualAmmo;

    //{ 
    //    get => _actualAmmo; 

    //    set => _actualAmmo = Mathf.Abs(value); 
    //}


    //[Header("Damage"),Tooltip("el daño base y actual del arma(deberia tener un damageManager?)")]
    //[SerializeField] int _baseDamage;
    //[SerializeField] public int _actualDamage;

    //[Header("Crit")]
    //[SerializeField] bool CanCrit;
    //[SerializeField] public float CritMultiplier;

    public DamageHandler damageHandler;

    public RateOfFireHandler rateFireHandler;

   
  

    [Header("Perks Stats"),Tooltip("perk equipados,(se deberia de hacer un PERK MANAGER creo," +
        " no estoy seguro)"),SerializeField]
     PerkHandler perkHandler;

    [SerializeField,Tooltip("Las estadisticas del arma (no se muestra una actualizacion in game " +
        "en caso de que se tenga un accesorio)")]
    public StatsHandler stats;


    //Des-comentar mas adelante, struct para clips de audios
    //[SerializeField] GunAudioClips Audioclips;

    [Header("Attachments")]
    public AttachmentHandler attachmentHandler;



    #region Events
    //estos eventos se usarian para callback hacia los perks y para feedback(particulas, sonidos,animaciones) 

    public event Action everyTick;

    public event Action onReload;

    public event Action onStow;

    public event Action onDraw;

    public event Action onShoot;

    public event Action<HitData> onHit;

    //en vez de tener un evento para cada una de estas condiciones, paso toda la info en un struct(hitData),
    //lo malo de esto es que va a haber llamados a los cual no les importe algo de lo q hay guardado en el struct,
    //por ej si le paso a la pool de particulas donde fue el impacto, no le va a importar si fue critico o no(o quizas si?)

    //internal event Action OnKill;

    //internal event Action OnCritHit;

    //internal event Action OnCritKill;


    #endregion

    public abstract void Shoot();
    public abstract bool ShootCondition();

    protected void OnHitCallBack(HitData data) => onHit?.Invoke(data);

    protected virtual void Awake()
    {

        stats.Initialize();
        damageHandler.initialize();
        //perkHandler.
        rateFireHandler.Initialize(this);
        attachmentHandler.Initialize(this);
        OptionalInitialize();
    }

    protected virtual void OptionalInitialize() { }

    private void Update() => everyTick?.Invoke();
    
    

    //lo ideal seria tener un metodo q llame a la animacion de recarga,
    //y que en algun frame la animacion de recarga llame a este metodo
    public void Reload()
    {
        _actualAmmo = (int)stats.myGunStats[StatNames.MaxMagazine];
        onReload?.Invoke();
    }

    /// <summary>
    /// dispara el arma, solo si no esta en cd y tiene balas
    /// </summary>
    public virtual void Trigger()
    { 
        //talvez deberia tener un metodo abstracto para las condiciones de disparo,
        //y cada arma eligiria cual quiere que sean sus condiciones para dispararse
        if (ShootCondition())
        {
            Shoot();
            onShoot?.Invoke();
            //_actualAmmo -= (int)stats.myGunStats[StatNames.AmooPerShoot];
        }

    }
    
    void Equip()
    {

    }

    public void Draw()
    {
        onDraw?.Invoke();
    }

    public void Stow()
    {
        onStow?.Invoke();
    }

}
