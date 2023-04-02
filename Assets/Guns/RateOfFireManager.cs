using UnityEngine;
//Manager de cadencia de tiro
[System.Serializable]
public class RateOfFireManager 
{
    //deberia tener un rate of fire manager para cada tipo de cadencia, uno para automatico/singleShot y otro para rafaga
    [SerializeField] float _actualRateOfFire;
    [SerializeField] float _rateOfFireCD;
    bool _canShoot;
    public bool canShoot=> _canShoot;

    public float actualRateOfFire => _actualRateOfFire;

    public void Initialize()
    {
        _rateOfFireCD = 0;
        _canShoot = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnterCooldown()
    {

    }
}
