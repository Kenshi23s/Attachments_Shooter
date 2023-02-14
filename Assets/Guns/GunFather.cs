using UnityEngine;
using System;

public abstract class GunFather : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator _gunAnim;

    //sonido inicial
    //sonido loop
    //sonido final

    [Header("Ammo")]
    [SerializeField] float baseMagazine;
    [SerializeField] public float _actualAmmo { get => _actualAmmo; private set => _actualAmmo = Mathf.Abs(value); }
    [SerializeField] float _maxAmmo;

    [Header("Damage")]
    [SerializeField] float _baseDamage;
    [SerializeField] public float _actualDamage;

    [Header("Crit")]
    [SerializeField] bool CanCrit;
    [SerializeField] public float CritMultiplier;

    [Header("RateOfFire")]
    [SerializeField] float rateOfFire;

    [Header("Perks Stats")]
    [SerializeField] Perk[] GunPeks;

    [Header("GunStats")]
    [SerializeField] protected float _range;
    [SerializeField] protected float _reloadSpeed;
    [SerializeField] protected float _handling;






    #region Events
    internal event Action OnReload;

    internal event Action OnStow;

    internal event Action OnDraw;

    internal event Action OnShoot;

    internal event Action OnKill;

    internal event Action OnHit;

    internal event Action OnCritHit;
    #endregion

    private void Awake()
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
        _actualAmmo = _maxAmmo;
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

   
   
}
