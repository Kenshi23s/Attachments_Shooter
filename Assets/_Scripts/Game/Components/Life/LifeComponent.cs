using System;
using UnityEngine;
using static TickEventsManager;
[RequireComponent(typeof(DebugableObject))]
public class LifeComponent : MonoBehaviour, IDamagable, IHealable
{
    DebugableObject _debug;

    [SerializeField] int _life = 100;
    [SerializeField] int _maxLife = 100;

    public int life => _life;
    public int maxLife => _maxLife;

    [SerializeField, Range(0.1f, 2)]
    float _dmgMultiplier = 1f;

    public bool ShowdamageNumber
    {
        get
        {
            return _showdamageNumber;
        }

        set
        {
            if (_showdamageNumber == value) return;

            if (value)
                OnTakeDamage += ShowDamageNumber;
            else
                OnTakeDamage -= ShowDamageNumber;

            _showdamageNumber = value;

        }
    }
    bool _showdamageNumber = true;

 


    [SerializeField] public bool canTakeDamage = true;
    [SerializeField] public bool canBeHealed = true;

    #region Events
    public event Action<int, int> OnHealthChange;
    public event Action OnHeal;

    //pasar TODA LA INFORMACION AL TOMAR DA�O (USAR EL STRUCT DE DAMAGE DATA)
    public event Action<int> OnTakeDamage;
    public event Action OnKilled;
    #endregion



    private void Awake()
    {
        
        _debug = GetComponent<DebugableObject>();
        // por si tenes hijos que pueden hacer de 
        foreach (var item in GetComponentsInChildren<HitableObject>()) item.SetOwner(this);
        #region SetEvents
        OnHeal += () => OnHealthChange?.Invoke(life, maxLife);
        OnTakeDamage += (x) => OnHealthChange?.Invoke(life, maxLife);
        OnTakeDamage += ShowDamageNumber;
        OnHealthChange?.Invoke(life, maxLife);
        #endregion
        enabled = false;

    }

    void ShowDamageNumber(int x)
    {
        
        FloatingTextManager.instance.PopUpText(x.ToString(), hitPos != Vector3.zero? hitPos : transform.position);
        hitPos = Vector3.zero;
    }
    Vector3 hitPos = Vector3.zero;

    public void SetHitPos(Vector3 x) => hitPos = x;

    public void SetNewMaxLife(int value) => _maxLife = Mathf.Clamp(value, 1, int.MaxValue);


    public void Initialize()
    {
        _life = _maxLife;
       
    }

    #region DamageSide

    public virtual DamageData TakeDamage(int dmgDealt,Vector3 hitPos)
    {
        this.hitPos = hitPos;
        return TakeDamage(dmgDealt);
    }

    public virtual DamageData TakeDamage(int dmgDealt)
    {
        DamageData data = new DamageData();
        if (!canTakeDamage) return data;

         dmgDealt = (int)(Mathf.Abs(dmgDealt) * _dmgMultiplier);
        _life -= dmgDealt; OnTakeDamage?.Invoke(dmgDealt);
        _debug.Log($" recibio {dmgDealt} de da�o ");

        if (_life <= 0)
        {
            OnKilled?.Invoke();
            data.wasKilled = true;
        }
        data.damageDealt = dmgDealt;
        return data;
    }

    public virtual void AddDamageOverTime(int TotalDamageToDeal, float TimeAmount)
    {
        int damagePerTick = Mathf.Max(1, (int)((TotalDamageToDeal / TimeAmount)));

        Action action = () =>
        {
            int dmgToDeal = life - damagePerTick > 0 ? damagePerTick : 0;
            TakeDamage(dmgToDeal);
        };

        AddHealthEvent(action, TimeAmount);
    }
    #endregion

    #region HealingSide
    /// <summary>
    /// a�ade vida, no supera la vida maxima
    /// </summary>
    /// <param name="HealAmount"></param>
    /// <returns></returns>
    public virtual int Heal(int HealAmount)
    {
        if (!canBeHealed) return 0;

        _debug.Log($" se curo {HealAmount} de vida ");
        _life += Mathf.Abs(HealAmount);

        OnHeal?.Invoke();
        if (_life > _maxLife) _life = _maxLife;

        return HealAmount;
    }
    /// <summary>
    /// A�ade x cantidad de vida al objetivo a lo largo de y segundos(no supera la vida maxima)
    /// </summary>
    /// <param name="totalHeal"></param>
    /// <param name="timeAmount"></param>
    public void AddHealOverTime(int totalHeal, float timeAmount)
    {
        int HealPerTick = (int)(totalHeal / timeAmount);   
        AddHealthEvent(() => Heal(HealPerTick), timeAmount);
    }
    #endregion

    
    void AddHealthEvent(Action action,float timeAmount)
    {
        TickEvent newHealthEvent = new TickEvent(); 

        newHealthEvent.OnTickAction= action;
        newHealthEvent.isTimeBased = true;
        newHealthEvent.timeStart = timeAmount;

        TickEventsManager.instance.AddAction(newHealthEvent);
    }

    
}



