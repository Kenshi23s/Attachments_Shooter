using System;
using UnityEngine;
using static StatsHandler;



[RequireComponent(typeof(Rigidbody))]
public abstract class BaseBulltet : MonoBehaviour
{

    Rigidbody _rb;
    // se llama cuando se impacta con un "Target"
    Action <HitData> _hitCallBack;

    // variables para pool
    #region
    //metodo de regreso
    Action<BaseBulltet, int> _poolReturn;
    //key
    int poolKey;
    #endregion

    public Gun myGun;

    BulletMovement myMovement;
    [SerializeField] BulletMovement.BulletMoveType myType;

    protected abstract void OnHitEffect(HitData hit);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // creo el tipo de movimiento q voy a usar
        myMovement = BulletMovement.MovementSelected(transform,_rb, myType);
        //test
       
    }

    private void Update() => myMovement?.MoveBullet();

    public virtual void Initialize(Action<BaseBulltet,int> _poolReturn,int poolKey)
    {    
        this._poolReturn = _poolReturn;
        this.poolKey = poolKey;     
    }

  
   public void SetGunAndDispatch(Gun myGun,Action<HitData> _hitCallBack)
   {
        this.myGun = myGun;
        myGun.onHit += OnHitEffect;
        myMovement.SetSpeed(myGun.stats.myGunStats[StatNames.BulletSpeed]);
       
       
        this._hitCallBack = _hitCallBack;

        Transform shootPos = myGun.attachmentHandler.shootPos;

        transform.position = shootPos.position; transform.forward = shootPos.forward;

        myMovement.Initialize();



    }

    public void Hit(IDamagable target)
    {
        
        DamageData dmgData = target.TakeDamage(myGun.damageHandler.actualDamage);

        HitData hit = new HitData(transform.position, dmgData, myGun);

        _hitCallBack?.Invoke(hit);
     
    }

   


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.TryGetComponent(out IDamagable x ))
        {
            Hit(x);
            Debug.Log("Damage");
        }      
            
        

        //me desuscribo de el evento y borro la referencia al callback
         myGun.onHit -= OnHitEffect;
        _hitCallBack = null;

        _rb.velocity = Vector3.zero;
        _poolReturn.Invoke(this, poolKey);
    }
   
}
