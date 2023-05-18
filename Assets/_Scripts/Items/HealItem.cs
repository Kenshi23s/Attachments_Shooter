using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(BoxCollider))]
public class HealItem : MonoBehaviour
{
    DebugableObject _debug;
    [SerializeField,Range(0.1f,30)] int HealAmount;
    [SerializeField,Range(0.1f, 30)] float timeToHeal;
    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        GetComponent<BoxCollider>().isTrigger = true;
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealable target))
        {        
            _debug.Log($"trato de curar a {target}");
            target.AddHealOverTime(HealAmount, timeToHeal);
            Destroy(gameObject);
        }
        _debug.Log($"{other} choco conmigo, no es Healable");
    }
}
