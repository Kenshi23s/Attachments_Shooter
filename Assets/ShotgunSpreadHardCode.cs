using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ShotgunSpreadHardCode : MonoBehaviour
{
    Attachment AttachmentOwner;
    public int pellets;

    Burst_Gun burst;

    private void Awake()
    {
        AttachmentOwner = GetComponent<Attachment>();
        AttachmentOwner.OnAttach += SetEvent;
        AttachmentOwner.OnDettach += UnbindEvent;
    }

    private void Start()
    {
      
    }

    void SetEvent()
    {
        burst = AttachmentOwner.owner.GetComponent<Burst_Gun>();
        burst.OnshootOnBurst += RepeatShotgunShot;
     
    }

    void UnbindEvent()
    {
        if (burst != null)
        {
            burst.OnshootOnBurst -= RepeatShotgunShot;
            burst = null;
        }
       
    }

    void RepeatShotgunShot()
    {
        burst.OnshootOnBurst -= RepeatShotgunShot;
       
        for (int i = 0; i < pellets; i++)
        {
            burst.ShootOnBurst();
        }
        burst.OnshootOnBurst += RepeatShotgunShot;
    }


}
