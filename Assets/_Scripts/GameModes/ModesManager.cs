
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

    //GameMode RandomGameMode()
    //{


    //   return GameMode randomGameMode= UnityEngine.Random.Range(0, Enum.GetValues.Length+1);

    //}
    protected override void SingletonAwake()
    {
      

        //if (actualGameMode == GameMode.Random)
        //{
        //    actualGameMode = RandomGameMode();
        //}

        //gameMode = SelectGameMode();

        gameMode.InitializeMode();
    }
   


    //GameModeBaseClass SelectGameMode()
    //{
    //    switch (actualGameMode)
    //    {
    //        case GameMode.EkillConfirm:
    //           return SearchGameMode<KillConfirmManager>();
               
    //        case GameMode.EhardPoint:
    //            return SearchGameMode<HardPointManager>();

    //    }
    //    return null;
    //}
    //GameModeBaseClass SearchGameMode<T>() where T : GameModeBaseClass
    //{
    //    foreach (T gameModeClass in AvailableGameModes)
    //    {
    //        return gameModeClass;
    //    }
    //    return null;
    //}

}
