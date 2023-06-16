using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public Player_Movement player;
    public GameObject HUD_Restart;

    private void Start()
    {
        player.lifehandler.OnKilled += PauseMethod;
      
    }

    public void PauseMethod()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ScreenManager.PauseGame();
        HUD_Restart.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ScreenManager.ResumeGame();
    }
}
