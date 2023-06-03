using UnityEngine;

public class Player_Handler : MonoBehaviour
{
    public GunManager myGunHandler { get; private set;}
    public LifeComponent myHealth { get; private set;}
    public Player_Movement myMovement { get; private set; }

    

    private void Awake()
    {
        myGunHandler = GetComponentInChildren<GunManager>();
        myGunHandler.SetPlayer(this);


        myHealth = GetComponent<LifeComponent>();
        myMovement = GetComponent<Player_Movement>();
    }

}
