using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModesManager : MonoSingleton<ModesManager>
{
    [SerializeField] GameModeBaseClass[] AvailableGameModes;
  
    public enum GameMode
    {
        EggChase,
        Random
      
    }
    [SerializeField]
    GameMode actualGameMode;

    public GameModeBaseClass actualMode { get; private set; }

    protected override void SingletonAwake()
    {

        actualMode = AvailableGameModes[0];
       
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;
        actualMode.gameObject.SetActive(true);
        actualMode.InitializeMode();
    }





}
