using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(BoxCollider))]
public class HealItem : MonoBehaviour
{
   
   
    DebugableObject _debug;
    [SerializeField,Range(1,100)]  int Amount;
  

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        GetComponent<BoxCollider>().isTrigger = true;
        Color a = default; 
        GetComponent<Renderer>().material.color = a;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealable target))
        {        
            _debug.Log($"trato de curar a {target}");
            target.Heal(Amount);        
            Destroy(gameObject);
            return;
        }       
        _debug.Log($"{other} choco conmigo, no es Idamagable ni Healable");
    }
}
