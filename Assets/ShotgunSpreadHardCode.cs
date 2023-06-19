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
        AttachmentOwner.onAttach += SetEvent;
        AttachmentOwner.onDettach += UnbindEvent;
    }

    private void Start()
    {
      
    }

    void SetEvent()
    {
        burst = AttachmentOwner.owner.GetComponent<Burst_Gun>();
        burst.shootOnBurst += RepeatShotgunShot;
     
    }

    void UnbindEvent()
    {
        if (burst != null)
        {
            burst.shootOnBurst -= RepeatShotgunShot;
            burst = null;
        }
       
    }

    void RepeatShotgunShot()
    {
        burst.shootOnBurst -= RepeatShotgunShot;
       
        for (int i = 0; i < pellets; i++)
        {
            burst.ShootOnBurst();
        }
        burst.shootOnBurst += RepeatShotgunShot;
    }


}
