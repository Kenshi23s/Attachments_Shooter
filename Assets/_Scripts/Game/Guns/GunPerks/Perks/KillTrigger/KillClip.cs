using UnityEngine;
public class KillClip : OverTimePerks
{
    bool condition;
    [SerializeField, Range(1.1f , 2f)]
    float multiplyBy;

    public override void InitializePerk(Gun gun)
    {
        myGun = gun;
        myGun.onHit += KillClipCondition;
        myGun.onReload += ActivateKillClip;      
       
    }

    void KillClipCondition(HitData data)
    {
        if (data.dmgData.wasKilled) 
        {
            condition= true;
            myGun.onHit -= KillClipCondition ;

        }
    }

    void ActivateKillClip() 
    {
        if (!is_Active && condition == true)
        {
            float newdmg = myGun.damageHandler.actualDamage * multiplyBy;
            myGun.damageHandler.IncreaseDamage((int)newdmg);

            condition = false;
            myGun.onHit -= KillClipCondition;

            TimerUpdate += CountDown;
        }
    }

    void CountDown(float time)
    {
        _actualTime -= time;

        if (_actualTime<=0)
        {
            TimerUpdate -= CountDown;
            myGun.onHit += KillClipCondition;
            is_Active = false;

          
        }
    }
    
}
