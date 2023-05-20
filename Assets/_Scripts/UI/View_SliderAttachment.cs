using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static Attachment;
using static StatsHandler;

public class View_SliderAttachment : MonoBehaviour
{
    float maxShowValue = 100f;
    [SerializeField] Slider positiveStat, negativeStat;
    Text statName;

    public void SetSliderValue(StatNames name,int value)
    {
        statName.text = name.ToString();

        if (0 > value)
        {
            negativeStat.value = 0;
            positiveStat.value = value * (1 / maxShowValue);
        }
        else
        {
            positiveStat.value = 0;
            negativeStat.value = value * (1 / -maxShowValue);
        }
    }

}
