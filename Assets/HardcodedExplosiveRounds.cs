using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodedExplosiveRounds : MonoBehaviour
{
    Attachment _owner;
    Gun gun;
    [SerializeField]
    Explosion _prefabExplosion;

    // Start is called before the first frame update
    void Awake()
    {
        _owner = GetComponent<Attachment>();
        _owner.onAttach += AttachExplosiveRounds;
        _owner.onDettach += DetachExplosiveRounds;
    }

    void AttachExplosiveRounds() 
    {
        gun = _owner.owner;
        if (gun!=null)
        {
            gun.onHit += InsantiateExplosiveRounds;
        }
      
    }

    void DetachExplosiveRounds() 
    {
        if (gun!=null)
        {
            gun.onHit -= InsantiateExplosiveRounds;
            gun = null;
        }
      
    }

    void InsantiateExplosiveRounds(HitData hitData) 
    {
        Instantiate(_prefabExplosion, hitData._impactPos, Quaternion.identity);
    }
}
