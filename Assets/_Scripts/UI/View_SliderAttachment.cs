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

    public AttachmentData GetData(Attachment attachment)
    {
        AttachmentData data = new AttachmentData();

        data.type = attachment.myType; data.name = attachment.name;

        foreach (StatNames key in Enum.GetValues(typeof(StatNames)))
            if (attachment.Attachment_stats.TryGetValue(key, out var x))
                data.stats.Add(key, x.value);

        return data;
    }

    public void SetSlider() 
    {

    }
}
