using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static Attachment;
using static StatsHandler;

public class View_SliderAttachment : MonoBehaviour
{
    Slider positiveStat, NegativeStat;
    Text statName;

    public struct AttachmentData
    {
        public AttachmentType type;
        public string name;
        public Dictionary<StatNames, int> stats;
    }

    public void SetSliderValue(StatNames name,int value)
    {

    }

}
