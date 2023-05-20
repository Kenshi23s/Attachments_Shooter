using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Attachment;
using static StatsHandler;

public class View_Attachment : MonoBehaviour
{
    GameObject Panel;
    public Text myTypeText;
    public Text nameText;
    public View_SliderAttachment sliderTemplate;

    public List<View_SliderAttachment> View_SliderAttachment;

    public struct AttachmentData
    {
        public AttachmentType type;
        public string name;
        public Dictionary<StatNames, int> stats;
    }
   
    public void NewAttachment(Attachment x)
    {
        transform.position= x.transform.position;

        View_SliderAttachment.Clear();

        AttachmentData data = GetData(x);
        myTypeText.text = data.type.ToString();
        nameText.text = data.name;

        foreach (StatNames key in data.stats.Keys)
        {
            View_SliderAttachment newSlider = Instantiate(sliderTemplate).GetComponent<View_SliderAttachment>();
            newSlider.transform.parent= Panel.transform;
            View_SliderAttachment.Add(newSlider);        
            newSlider.SetSliderValue(key, data.stats[key]);
        }
    }

    void AddSlider()
    {

    }

    AttachmentData GetData(Attachment attachment)
    {
        AttachmentData data = new AttachmentData();

        data.type = attachment.myType; data.name = attachment.name;

        foreach (StatNames key in Enum.GetValues(typeof(StatNames)))
            if (attachment.Attachment_stats.TryGetValue(key, out var x))
                data.stats.Add(key, x.value);

        return data;
    }
  
}
