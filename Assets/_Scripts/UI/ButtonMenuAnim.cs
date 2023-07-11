using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMenuAnim : MonoBehaviour
{
    public Image fillArrowLeft, fillArrowRight;
    public Image emptyArrowLeft, emptyArrowRight;
    float speed = 0.1f;
    

    public void MoveArrows()
    {
        //Vector3 dirLeft = emptyArrowLeft.transform.position - fillArrowLeft.transform.position;

        //fillArrowLeft.transform.position -= dirLeft * speed * Time.deltaTime;

        fillArrowLeft.transform.position = emptyArrowLeft.transform.position;
        fillArrowRight.transform.position = emptyArrowRight.transform.position;
        
    }
}
