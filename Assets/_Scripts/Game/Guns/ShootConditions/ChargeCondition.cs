using System;
using UnityEngine;

public class ChargeCondition : ShootCondition
{

   public event Action WhileCharging, WhileDecharging, OnChargeTimeMet;

   [field : SerializeField] public float CurrentCharge { get; protected set; }

   [field : SerializeField] public float ChargeTimeScalar { get; protected set; }

   [field : SerializeField] public float DeChargeTimeScalar { get; protected set; }

   [field : SerializeField] public float MaxCharge { get; protected set; }

   public bool EmptyChargeOnConditionsMet;

    //nesecito nombres mas cortos :C
    [field : SerializeField] public float RemovePercentageOnConditionsMet { get; protected set; }




    public override bool ShootConditionsMet()
    {
        //si el mouse esta apretado Cargo
        if (Input.GetKey(KeyCode.Mouse0))
        {
            CurrentCharge += Time.deltaTime * ChargeTimeScalar;
            WhileCharging?.Invoke();
        }
        else if(CurrentCharge > 0) // si no esta apretado y la carga es > a 0 descargo
        {
            CurrentCharge -= Time.deltaTime * DeChargeTimeScalar;
            CurrentCharge = Mathf.Max(CurrentCharge, 0);
            WhileDecharging?.Invoke();
        }


        if (WasChargeTimeMet())
        {
            OnChargeTimeMet?.Invoke();

            if (EmptyChargeOnConditionsMet) CurrentCharge = 0;

            else CurrentCharge = RemovePercentageOnConditionsMet * CurrentCharge / 100;

            return true;
        }
        return false;
    }



    public virtual bool WasChargeTimeMet()
    {
        return CurrentCharge >= MaxCharge;    
    }
    
       
    
   
}
