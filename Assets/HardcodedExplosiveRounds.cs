using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodedExplosiveRounds : MonoBehaviour
{
    Attachment _owner;
    [SerializeField]
    Explosion _prefabExplosion;

    // Start is called before the first frame update
    void Awake()
    {
        _owner = GetComponent<Attachment>();
        _owner.onAttach += AttachExplosiveRounds;
        _owner.onDettach -= DetachExplosiveRounds;
    }

    void AttachExplosiveRounds() 
    {
        _owner.owner.onHit += InsantiateExplosiveRounds;
    }

    void DetachExplosiveRounds() 
    {
        _owner.owner.onHit -= InsantiateExplosiveRounds;
    }

    void InsantiateExplosiveRounds(HitData hitData) 
    {
        Instantiate(_prefabExplosion, hitData._impactPos, Quaternion.identity);
    }
}
