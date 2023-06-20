using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
[RequireComponent(typeof(BoxCollider))]
public class HealItem : MonoBehaviour
{
   
   
    DebugableObject _debug;
    [SerializeField,Range(1,100)]  int Amount;

   
    [SerializeField]float degreesPerSecond;

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        GetComponent<BoxCollider>().isTrigger = true;
    
        

    }

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, degreesPerSecond*Time.deltaTime, 0);


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
