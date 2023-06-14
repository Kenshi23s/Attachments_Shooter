using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FacundoColomboMethods;

public class EDog_Turret : MonoBehaviour
{
    [SerializeField]E_ExplosiveDog owner;


    [SerializeField]float rateOfFire, triggerDistance;
    [SerializeField]int ammo;
    [SerializeField]int damage;

   [SerializeField] TrailRenderer trail;
    // Start is called before the first frame update

    private void Awake()
    {
        owner = GetComponentInParent<E_ExplosiveDog>();
       
;   }

    private void Start()
    {
        StartCoroutine(TurretFinishCondition());
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(rateOfFire);
        while (true)
        {
            enabled= false;
            yield return new WaitUntil(() => owner.agent.FOV.IN_FOV(Player_Movement.position,1000f));
            enabled = true;
            Shoot();
            yield return wait;

        }
      
    }

    private void Update()
    {
        //Vector3 dir = Player_Movement.position -transform.position;
        //transform.forward = new Vector3(dir.x,0,dir.z);
    }

    void Shoot()
    {
        
        Vector3 dir = transform.forward.RandomDirFrom(45f);
        Vector3 impactPos = Vector3.zero;
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit))
        {
            impactPos = hit.point;
            if (hit.transform.TryGetComponent(out IDamagable x))           
                x.TakeDamage(damage);

            if (hit.transform.TryGetComponent(out IHitFeedback y))
            {
                y.FeedbackHit(hit.point,hit.normal);
            }
        }
        else       
            impactPos = dir * 100;
        
        var aux = Instantiate(trail, transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(aux, impactPos));


        ammo--;
        
    }

    IEnumerator TurretFinishCondition()
    {
        float aux = owner.agent.Movement.maxForce;
        owner.agent.Movement.maxForce = 0;
        yield return new WaitUntil(() =>
        {
            return ammo <= 0 || Vector3.Distance(Player_Movement.position, transform.position) < triggerDistance;
        });

        owner.agent.Movement.maxForce = aux;
    }

    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 impactPos)
    {
        Vector3 startPos = trail.transform.position;
        float dist = Vector3.Distance(startPos, impactPos);
        float time = 0f;

        while (time < 1)
        {

            trail.transform.position = Vector3.Lerp(startPos, impactPos, time);


            time += (Time.deltaTime / trail.time) * (dist / Vector3.Distance(trail.transform.position, impactPos));
            yield return null;
        }
        trail.transform.position = impactPos;
        Destroy(trail.gameObject);
    }
}
