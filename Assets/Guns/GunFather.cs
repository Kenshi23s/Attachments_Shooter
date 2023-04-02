using UnityEngine;
using System;
using System.Collections.Generic;
using static GunStats;
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
    public Vector3 Pos;
    public IDamagable Target;
    public int dmgDealt;
    public GunFather weapon;

    public HitData(Vector3 pos, IDamagable target, int dmgDealt, GunFather weapon)
    {
        Pos = pos;
        Target = target;
        this.dmgDealt = dmgDealt;
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

    public GunDamageManager damageManager;

    public RateOfFireManager rateFireManager;

   
  

    [Header("Perks Stats"),Tooltip("perk equipados,(se deberia de hacer un PERK MANAGER creo," +
        " no estoy seguro)"),SerializeField]
     PerkManager perkManager;

    [SerializeField,Tooltip("Las estadisticas del arma (no se muestra una actualizacion in game " +
        "en caso de que se tenga un accesorio)")]
    public GunStats stats;


    //Des-comentar mas adelante, struct para clips de audios
    //[SerializeField] GunAudioClips Audioclips;

    [Header("Attachments")]
    public AttachmentManager attachMents;


   
    #region Events
    //estos eventos se usarian para callback hacia los perks y para feedback(particulas, sonidos,animaciones) 
    internal event Action OnReload;

    internal event Action OnStow;

    internal event Action OnDraw;

    internal event Action OnShoot;

    //internal event Action OnKill;

    internal event Action<HitData> OnHit;

    //internal event Action OnCritHit;

    //internal event Action OnCritKill;


    #endregion

    public abstract void Shoot();

    protected void OnHitCallBack(HitData data) => OnHit?.Invoke(data);

    protected virtual void Awake()
    {

        damageManager.initialize();
        rateFireManager.Initialize();
        stats.Initialize();
        //perkManager.Initialize(this);
        attachMents.Initialize(this);
        OptionalInitialize();
    }

    protected virtual void OptionalInitialize() { }
   
   
    
    //lo ideal seria tener un metodo q llame a la animacion de recarga,
    //y que en algun frame la animacion de recarga llame a este metodo
    public void Reload()
    {
        _actualAmmo = (int)stats.myGunStats[StatNames.MaxMagazine];
        OnReload?.Invoke();
    }

    /// <summary>
    /// dispara el arma, solo si no esta en cd y tiene balas
    /// </summary>
    public virtual void Trigger()
    {
        //solo para testo no esta pasando por las condiciones
        Shoot();
        //if (_actualAmmo >= stats.myGunStats[StatNames.AmooPerShoot] && rateFireManager.canShoot)
        //{
        //    Shoot();
        //    _actualAmmo -= (int)stats.myGunStats[StatNames.AmooPerShoot];
        //    OnShoot?.Invoke();
        //}
      
    }
    
    void Equip()
    {

    }

    public void Draw()
    {
        OnDraw?.Invoke();
    }

    public void Stow()
    {
        OnStow?.Invoke();
    }

}
