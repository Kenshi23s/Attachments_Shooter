using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextAndFiller : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Slider slider;

    public void SetText(string x)
    {
        text.text = x;
    }

    public void SetSliderValue(float x)
    {
        slider.value = Mathf.Clamp01(x);
    }
}
