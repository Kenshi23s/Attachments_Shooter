using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(BoxCollider))]
public class HealItem : MonoBehaviour
{
    public enum Function
    {
        Damage=0,
        Heal=1
    }
    [SerializeField] Function function;
    DebugableObject _debug;
    [SerializeField,Range(0.1f,10)]  int Amount;
    [SerializeField,Range(0.1f, 10)] float time;

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        GetComponent<BoxCollider>().isTrigger = true;

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealable target) && Function.Heal==function)
        {        
            _debug.Log($"trato de curar a {target}");
            target.AddHealOverTime(Amount, time);
            Destroy(gameObject);
        }

        if (other.TryGetComponent(out IDamagable a) && function == Function.Damage)
        {
            _debug.Log($"trato de hacer daño a {a}");
            a.AddDamageOverTime(Amount, time);
            Destroy(gameObject);
        }
        _debug.Log($"{other} choco conmigo, no es Idamagable ni Healable");
    }
}
