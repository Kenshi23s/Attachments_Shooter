using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextAndFiller : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Text textOverSlider;
    [SerializeField] Slider slider;


    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.position - transform.position;
    }

    public void SetFontColor(Color color)
    {
        text.color = color;
    }
    public void SetText(string x)
    {
        text.text = x;
    }

    public void SetOverlayText(string x)
    {
        textOverSlider.text = x;
    }

    public void TurnSliderTextOff()
    {
        textOverSlider.gameObject.SetActive(false);
        slider.gameObject.SetActive(true);
    }

    public void TurnSliderTextOn()
    {
        textOverSlider.gameObject.SetActive(true);
        slider.gameObject.SetActive(false);
    }

    public void SetSliderValue(float x)
    {
        slider.value = Mathf.Clamp01(x);
    }
}
