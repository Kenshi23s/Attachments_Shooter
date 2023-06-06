
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModesManager : MonoSingleton<ModesManager>
{
    [SerializeField] GameModeBaseClass[] AvailableGameModes;
    [Tooltip("solo lectura, no modificar")]
    public GameModeBaseClass gameMode;
    public enum GameMode
    {
        EggChase,
        Random
      
    }
    [SerializeField]
    GameMode actualGameMode;


    protected override void SingletonAwake()
    {

        gameMode = AvailableGameModes[0];
       
    }

    private void Start()
    {
        gameMode.gameObject.SetActive(true);
        gameMode.InitializeMode();
    }





}
