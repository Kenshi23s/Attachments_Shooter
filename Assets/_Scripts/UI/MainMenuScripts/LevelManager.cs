using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _menu;
    [SerializeField] private Image _progressBar;
    private float _target;

    List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    public void StartGame()
    {
        HideMenu();
        ShowLoadingScreen();
        _scenesToLoad.Add(SceneManager.LoadSceneAsync("Level1_New"));
        StartCoroutine(LoadingScreen());
    }

    void HideMenu()
    {
        _menu.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        _loadingScreen.SetActive(true);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        for(int i = 0; i < _scenesToLoad.Count; i++)
        {
            while (!_scenesToLoad[i].isDone)
            {
                totalProgress += _scenesToLoad[i].progress;
                _progressBar.fillAmount = totalProgress/_scenesToLoad.Count;
                yield return null;
            }
        }
    }

}
