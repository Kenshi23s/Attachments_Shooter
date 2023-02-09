using UnityEngine;
public class KillClip : OverTimePerks
{
    bool condition;
    [SerializeField, Range(1.1f , 2f)]
    float multiplyBy;

    internal override void InitializePerk(GunFather gun)
    {
        myGun = gun;
        myGun.OnKill += KillClipCondition;
        myGun.OnReload += ActivateKillClip;      
       
    }

    void KillClipCondition() => condition = true;

    void ActivateKillClip() 
    {
        if (!is_Active && condition == true)
        {
            float newdmg = myGun._actualDamage * multiplyBy;
            myGun.AddDamage(newdmg);

            condition = false;
            myGun.OnKill -= KillClipCondition;

            TimerUpdate += CountDown;
        }
    }

    void CountDown(float time)
    {
        _actualTime -= time;

        if (_actualTime<=0)
        {
            TimerUpdate -= CountDown;
            myGun.OnKill += KillClipCondition;
            is_Active = false;

          
        }
    }
    
}
