using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    static AsyncOperation _LoadOperation;
    static int preLoadedLevel;
    void LateUpdate()
    {
        if (_LoadOperation.isDone)
        {
            _LoadOperation.allowSceneActivation = true;
            _LoadOperation = null;
        }
    }

    public static void LoadASyncLevel(int index)
    {
        SceneManager.LoadScene(2);
        
     

        _LoadOperation = SceneManager.LoadSceneAsync(index);
        _LoadOperation.allowSceneActivation = false;
    }

    public static IEnumerator StartPreLoadingLevel(int index)
    {
        _LoadOperation = SceneManager.LoadSceneAsync(index);
        _LoadOperation.allowSceneActivation = false;
        preLoadedLevel = index;
        yield return new WaitUntil(() => _LoadOperation.allowSceneActivation);
    }


}
