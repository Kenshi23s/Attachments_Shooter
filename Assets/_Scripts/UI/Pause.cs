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
        Time.timeScale = 1;
    }

    public void PauseMethod()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        HUD_Restart.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
