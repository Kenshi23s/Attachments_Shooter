using UnityEngine;

public class Player_Handler : MonoBehaviour
{
    public GunHandler myGunHandler { get; private set;}
    public LifeComponent myHealth { get; private set;}
    public Player_Movement myMovement { get; private set; }

    

    private void Awake()
    {
        myGunHandler = GetComponentInChildren<GunHandler>();
        myGunHandler.SetPlayer(this);


        myHealth = GetComponent<LifeComponent>();
        myMovement = GetComponent<Player_Movement>();
    }

}
