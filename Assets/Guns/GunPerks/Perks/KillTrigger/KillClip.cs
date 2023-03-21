using UnityEngine;
public class KillClip : OverTimePerks
{
    bool condition;
    [SerializeField, Range(1.1f , 2f)]
    float multiplyBy;

    internal override void InitializePerk(GunFather gun)
    {
        myGun = gun;
        myGun.OnHit += KillClipCondition;
        myGun.OnReload += ActivateKillClip;      
       
    }

    void KillClipCondition(HitData data)
    {
        if (data.Target.WasKilled()) 
        {
            condition= true;
            myGun.OnHit -= KillClipCondition ;

        }
    }

    void ActivateKillClip() 
    {
        if (!is_Active && condition == true)
        {
            float newdmg = myGun._actualDamage * multiplyBy;
            myGun.AddDamage((int)newdmg);

            condition = false;
            myGun.OnHit -= KillClipCondition;

            TimerUpdate += CountDown;
        }
    }

    void CountDown(float time)
    {
        _actualTime -= time;

        if (_actualTime<=0)
        {
            TimerUpdate -= CountDown;
            myGun.OnHit += KillClipCondition;
            is_Active = false;

          
        }
    }
    
}
