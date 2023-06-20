using FacundoColomboMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using UnityEngine;


public class HardcodedBulletBounce : MonoBehaviour
{
    Attachment attachment;
    RaycastComponent raycastComponent;
    Gun GunRef;

    private void Awake()
    {
        attachment = GetComponent<Attachment>();
        attachment.onAttach += OnAttach;
        attachment.onDettach += OnDetach;
    }

    void OnAttach()
    {
        raycastComponent = attachment.owner.GetComponent<RaycastComponent>();
        GunRef = attachment.owner;
        GunRef.onHit += Bounce;
    }

    void OnDetach()
    {
        if (GunRef!=null)
        {
            GunRef.onHit -= Bounce;
        }
      
        if (attachment.owner!=null)
        {
            attachment.owner.onHit -= Bounce;
            raycastComponent = null;
        }
       
    }

    void Bounce(HitData hit)
    {
        GunRef.onHit -= Bounce;

        IDamagable enemy = hit._impactPos.GetItemsOFTypeAround<IDamagable>(1000f).Where(x=>x!=hit.dmgData.victim)
            .Where(x => hit._impactPos.InLineOffSight(x.Position(), AI_Manager.instance.wall_Mask))
            .Minimum(x=>Vector3.Distance(x.Position(), hit._impactPos));
        Debug.Log("enter bounce");
        if (enemy!=null)
        {
            Debug.Log("hit clone");
            Vector3 spawnPos = hit._impactPos + Vector3.up * 2f;
            Vector3 dir = enemy.Position() - spawnPos;
            raycastComponent.ShootRaycast(spawnPos, dir, attachment.owner.OnHitCallBack);

        }
        GunRef.onHit += Bounce;
    }
}
