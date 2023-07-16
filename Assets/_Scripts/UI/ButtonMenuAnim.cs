using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMenuAnim : MonoBehaviour
{
    public Image fillArrowLeft, fillArrowRight;
    public Image emptyArrowLeft, emptyArrowRight;
    float speed = 0.1f;
    Vector2 initialPosLeftFillArrow;
    Vector2 initialPosRightFillArrow;

    private void Start()
    {
        initialPosLeftFillArrow = fillArrowLeft.transform.position;
        initialPosRightFillArrow = fillArrowRight.transform.position;
    }


    public void MoveArrows()
    {
        

        //fillArrowLeft.transform.position -= dirLeft * speed * Time.deltaTime;

        fillArrowLeft.transform.position = emptyArrowLeft.transform.position;
        fillArrowRight.transform.position = emptyArrowRight.transform.position;
        
    }

    public void NormalArrows()
    {
        fillArrowLeft.transform.position = initialPosLeftFillArrow;
        fillArrowRight.transform.position= initialPosRightFillArrow;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
