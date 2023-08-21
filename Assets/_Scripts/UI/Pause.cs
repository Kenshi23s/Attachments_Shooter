using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public Player_Handler player;
    public GameObject HUD_Restart;
    public Transform respawnPoint;

    private void Start()
    {
        player.Health.OnKilled.AddListener(PauseMethod);
      
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
        ScreenManager.ResumeGame();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        HUD_Restart.SetActive(false);
        player.transform.position = respawnPoint.position;
        player.Health.Heal(int.MaxValue/2);
    }
}
