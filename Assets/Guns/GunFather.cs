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


    [Header("Damage")]
    [SerializeField] int _baseDamage;
    [SerializeField] public int _actualDamage;

    //[Header("Crit")]
    //[SerializeField] bool CanCrit;
    //[SerializeField] public float CritMultiplier;

    [Header("RateOfFire")]
    [SerializeField] float _actualRateOfFire;
    [SerializeField] float rateOfFireCD;

    [Header("Perks Stats")]
    [SerializeField] Perk[] GunPeks;

    [SerializeField]
    public GunStats _myStats;


    //Des-comentar mas adelante
    //[SerializeField] GunAudioClips Audioclips;

    [Header("Attachments")]
    public AttachmentManager _myAttachMents;

   

    #region Events
    internal event Action OnReload;

    internal event Action OnStow;

    internal event Action OnDraw;

    internal event Action OnShoot;

    //internal event Action OnKill;

    internal event Action<HitData> OnHit;

    //internal event Action OnCritHit;

    //internal event Action OnCritKill;

    
    #endregion

    protected void OnHitCallBack(HitData data) => OnHit?.Invoke(data);

    protected virtual void Awake()
    {
        _actualDamage = _baseDamage;

        rateOfFireCD = 0;

        foreach (Perk item in GunPeks)
        {
            item.InitializePerk(this);
        }
        
        _myStats.Initialize();
        _myAttachMents.Initialize(this);
        OptionalInitialize();
    }

    protected virtual void OptionalInitialize() { }
   
    public void AddDamage(int value) => _actualDamage += value;
    
    public void SubstractDamage(int value) => _actualDamage -= value;
    

    public void Reload()
    {
        _actualAmmo = (int)_myStats.myGunStats[StatNames.MaxMagazine];
        OnReload?.Invoke();
    }

    //public abstract void PreShoot();

    public abstract void Shoot();

    //public abstract void PosShoot();

    public virtual void Trigger()
    {
        Shoot();
        if (_actualAmmo >= _myStats.myGunStats[StatNames.AmooPerShoot] && rateOfFireCD<=0)
        {
             
            _actualAmmo -= (int)_myStats.myGunStats[StatNames.AmooPerShoot];
            OnShoot?.Invoke();
        }
      
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

    //void DoDamage(IDamagable target)
    //{
    //    target.TakeDamage(_actualDamage);
    //}

    private void OnDrawGizmos()
    {
        //if (_stats.shootPos!=null)
        //{
        //    Gizmos.DrawLine(_stats.shootPos.position, _stats.shootPos.position + _stats.shootPos.forward * 11);
        //}
        
    }
}
