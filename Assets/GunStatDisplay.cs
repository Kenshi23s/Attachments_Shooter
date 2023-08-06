using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunStatDisplay : MonoBehaviour
{
    public Slider SliderStat;
    public TextMeshProUGUI StatName;

    public void SetStatDisplay(StatsHandler.StatNames name,float statValue = 0)
    {
        Debug.Log(statValue);
        SliderStat.value = Mathf.Max(0, statValue/StatsHandler.MaxStatValue);
        StatName.text = name.ToString();
    }

}
