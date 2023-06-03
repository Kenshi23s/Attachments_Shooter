using UnityEngine;

public class Player_Handler : MonoBehaviour
{
    public GunManager mygunHandler { get; private set;}
    public LifeComponent myHealth { get; private set;}
    public Player_Movement myMovement { get; private set; }

    

    private void Awake()
    {
        mygunHandler = GetComponentInChildren<GunManager>();
        mygunHandler.SetPlayer(this);


        myHealth = GetComponent<LifeComponent>();
        myMovement = GetComponent<Player_Movement>();
    }

}
