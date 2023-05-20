using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static StatsHandler;
using static View_SliderAttachment;

public class View_Attachment : MonoBehaviour
{

    public Text myTypeText;
    public Text nameText;
    public View_SliderAttachment sliderTemplate;

    public List<View_SliderAttachment> View_SliderAttachment;


    public void NewAttachment(Attachment x)
    {
        AttachmentData data = GetData(x);
        myTypeText.text = x.name
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
