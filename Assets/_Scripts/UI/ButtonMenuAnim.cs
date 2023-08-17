using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class ButtonMenuAnim : MonoBehaviour
{
    public Image fillArrowLeft, fillArrowRight;
    public Image emptyArrowLeft, emptyArrowRight;
    float speed = 0.1f;
    Vector3 initialPosLeftFillArrow;
    Vector3 initialPosRightFillArrow;
 
    private void Start()
    {
        initialPosLeftFillArrow = fillArrowLeft.transform.position;
        initialPosRightFillArrow = fillArrowRight.transform.position;
    }


    public void MoveArrows()
    {
        fillArrowLeft.transform.position = emptyArrowLeft.transform.position;
        fillArrowLeft.transform.rotation = emptyArrowLeft.transform.rotation;
        fillArrowRight.transform.position = emptyArrowRight.transform.position;
        //
    }

    public void NormalArrows()
    {
        fillArrowLeft.transform.position = initialPosLeftFillArrow;
        fillArrowRight.transform.position= initialPosRightFillArrow;
    }

    public void PlayGame()
    {
        LoadSceneManager.LoadASyncLevel(1);
    }

    public void TestUIScene()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.LogWarning("Quit");
        Application.Quit();
    }
    
}
