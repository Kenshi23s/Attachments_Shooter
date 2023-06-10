using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class HitBox : MonoBehaviour
{
    public GameObject owner { get; private set; }
    HashSet<IDamagable> alreadydmged = new HashSet<IDamagable>();

    BoxCollider hitbox;

    public event Action<HitData> onHit;

    public int damage { get; private set; }

    public Vector3 size { get; private set; }

    private void Awake() => hitbox = GetComponent<BoxCollider>();
  

    public void ActivateHitbox() => gameObject.SetActive(true);
       

    public void DeactivateHitBox()
    {
        alreadydmged.Clear();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other!=owner&&other.TryGetComponent(out IDamagable target))       
        if (!alreadydmged.Contains(target))
        {
                DamageData data = target.TakeDamage(damage); alreadydmged.Add(target);
                HitData hit = new HitData();
                hit._impactPos = other.ClosestPoint(other.transform.position);
                hit.dmgData = data;
                onHit?.Invoke(hit);
        }
        
    }

    public void SetSize(Vector3 newSize) => size = Vector3.Max(newSize, Vector3.zero);
    public void SetDamage(int newDamage) => damage = Mathf.Min(newDamage, 1);
    public void SetOwner(GameObject newOwner) => owner =newOwner;

}
