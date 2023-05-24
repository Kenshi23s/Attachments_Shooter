using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Attachment;
using static StatsHandler;

public class View_Attachment : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    public Text myTypeText;
    public Text nameText;
    public View_SliderAttachment sliderTemplate;

    public List<View_SliderAttachment> View_SliderAttachment;
    Attachment actual;

    public struct AttachmentData
    {
        public AttachmentType type;
        public string name;
        public Dictionary<StatNames, int> stats;
    }

    private void LateUpdate()
    {
        //saco la direccion opuesta para q lo vea bien(????)
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.forward=new Vector3(dir.x,0,dir.z);
    }

    public void NewAttachment(Attachment x)
    {
        if (x == null) return;
       
        transform.position = x.transform.position;
        if (actual == x) return;
        actual = x; RemoveStats();

        AttachmentData data = GetData(x);
        myTypeText.text = data.type.ToString();
        nameText.text = data.name;
      
        foreach (StatNames key in data.stats.Keys.Where(x => data.stats[x] != 0))
        {
            View_SliderAttachment newSlider = Instantiate(sliderTemplate, Panel.transform).GetComponent<View_SliderAttachment>();
            View_SliderAttachment.Add(newSlider);         
            newSlider.SetSliderValue(key.ToString(), data.stats[key]);
        }
    }

    void RemoveStats()
    {
        foreach (View_SliderAttachment item in View_SliderAttachment) Destroy(item.gameObject);      
        View_SliderAttachment.Clear();
    }

    AttachmentData GetData(Attachment attachment)
    {
        AttachmentData data = new AttachmentData();

        data.type = attachment.myType; data.name = attachment.name;

        data.stats = new Dictionary<StatNames, int>();

        foreach (StatNames key in Enum.GetValues(typeof(StatNames)))
            if (attachment.Attachment_stats.TryGetValue(key, out var x))
                data.stats.Add(key, x.value);

        return data;
    }
  
}
