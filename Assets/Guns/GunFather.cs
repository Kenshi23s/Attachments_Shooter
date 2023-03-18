using UnityEngine;
using System;
using System.Collections.Generic;

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
[System.Serializable]
public struct GunStats
{
    //quien deberia hacer el cambio de estadistica, el arma o el accesorio?
    //el arma deberia tener referencia de todos sus accesorios o no es de gran importancia?
    [Header("GunStats")]
    [SerializeField, Range(1,100)]  float _aimingZoom;
    [SerializeField, Range(1, 100)]  float _range;

    [SerializeField, Range(1, 100)]  float _handling;
    [SerializeField, Range(1, 100)]  float _stability;

    [SerializeField, Range(1, 100)]  float _reloadSpeed;
    [SerializeField, Range(1, 100)]  float _maxMagazineAmmo;

    public float aimingZoom => _aimingZoom;
    public float range => _range;

    public float handling => _handling;
    public float stability => _stability;

    public float reloadSpeed => _reloadSpeed;
    public float maxMagazineAmmo => _maxMagazineAmmo;

    Transform shootPos;





    #region StatMethods
    private void AddAimZoom(float value)     => _aimingZoom += Mathf.Clamp(value, 1, 100);
    private void AddRange(float value)       => _range += Mathf.Clamp(value,1,value);
  
    private void AddReloadSpeed(float value) => _reloadSpeed += Mathf.Clamp(value, 1, 100);
    private void AddHandling(float value)    => _handling += Mathf.Clamp(value, 1, 100);
   
    private void AddStability(float value)   => _stability += Mathf.Clamp(value, 1, 100);
    private void AddMaxMagCapacity(float value)   => _stability += Mathf.Clamp(value, 1, 100);
    #endregion


    
    /// <summary>
    /// cambia las stats del arma, si el booleano se pasa como verdadero las armas aumentan sus stats.
    /// en caso contrario las reduce
    /// </summary>
    /// <param name="NewStats"></param>
    /// <param name="_isBeingAttached"></param>
    public void ChangeStats(GunStats NewStats,bool _isBeingAttached)
    {
        int x = _isBeingAttached ? 1 : -1;
        
       
        AddAimZoom(NewStats.aimingZoom * x);
        AddRange(NewStats._range * x);

        AddReloadSpeed(NewStats.reloadSpeed * x);
        AddHandling(NewStats.handling * x);

        AddStability(NewStats.stability * x);
        AddMaxMagCapacity(NewStats.maxMagazineAmmo * x);

        OnStatsChange?.Invoke(this);
    }

    event Action<GunStats> OnStatsChange;
}


public abstract class GunFather : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator _gunAnim;


    [SerializeField] public float _actualAmmo 
    { 
        get => _actualAmmo; 

        private set => _actualAmmo = Mathf.Abs(value); 
    }
    

    [Header("Damage")]
    [SerializeField] float _baseDamage;
    [SerializeField] public float _actualDamage;

    [Header("Crit")]
    [SerializeField] bool CanCrit;
    [SerializeField] public float CritMultiplier;

    [Header("RateOfFire")]
    [SerializeField] float _actualRateOfFire;

    [Header("Perks Stats")]
    [SerializeField] Perk[] GunPeks;
    

    public GunStats Stats;

    [SerializeField]GunAudioClips clips;

    [Header("Attachments")]
    [SerializeField] Sight mySight;
    [SerializeField] Magazine myMagazine;
    [SerializeField] Muzzle myMuzzle;

    Transform defaultShootPos;
    #region Events
    internal event Action OnReload;

    internal event Action OnStow;

    internal event Action OnDraw;

    internal event Action OnShoot;

    internal event Action OnKill;

    internal event Action OnHit;

    internal event Action OnCritHit;

    internal event Action OnCritKill;

    
    #endregion

    protected virtual void Awake()
    {
        _actualDamage = _baseDamage;

        foreach (Perk item in GunPeks)
        {
            item.InitializePerk(this);
        }
        OptionalInitialize();
    }

    protected virtual void OptionalInitialize() { }
   
    public void AddDamage(float value) => _actualDamage += value;
    
    public void SubstractDamage(float value) => _actualDamage -= value;
    

    public void Reload()
    {
        _actualAmmo = Stats.maxMagazineAmmo;
        OnReload?.Invoke();
    }

    //public abstract void PreShoot();

    public abstract void GunShoot();

    //public abstract void PosShoot();

    public virtual void Shoot()
    {
        GunShoot();
        OnShoot?.Invoke();
    }

    void DoDamage(IDamagable target)
    {
        target.TakeDamage(_actualDamage);
    }
   
}
