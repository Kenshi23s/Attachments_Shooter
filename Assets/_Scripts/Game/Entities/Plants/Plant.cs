using UnityEngine;
[RequireComponent(typeof(BurnableComponent))]
public class Plant : MonoBehaviour
{
    public BurnableComponent BurnableC { get; private set; }
    [field: SerializeField]
    public float InitialHealth { get; private set; } = 100;
    public float CurrentHealth { get; private set; }

    public float LifePerBurnTick 
        => BurnableC.BurnStacks * BurnableC.DecreaseBurnSpeed;
    private void Awake()
    {
        BurnableC = GetComponent<BurnableComponent>();
        CurrentHealth = InitialHealth;
        BurnableC.OnBurn.AddListener(UpdateHealth);

    }

    void UpdateHealth()
    {     
        CurrentHealth -= Mathf.Max(0,LifePerBurnTick) ;
        if (CurrentHealth <= 0)
            Destroy(gameObject);
        
    }
}
