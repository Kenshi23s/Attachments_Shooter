using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Attachment;
using static StatsHandler;
[RequireComponent(typeof(DebugableObject))]
public class View_Attachment : MonoBehaviour
{
    //[SerializeField] GameObject Panel;
    public Text myTypeText;
    public Text nameText;
    public View_SliderAttachment sliderTemplate;
    DebugableObject _debug;
    public List<View_SliderAttachment> View_SliderAttachment;
    Attachment _actual;

    RectTransform _rectTransform;

    public struct AttachmentData
    {
        public AttachmentType type;
        public string name;
        public Dictionary<StatNames, int> stats;
    }

    private void LateUpdate()
    {
        //saco la direccion opuesta para q lo vea bien(????)
        //Vector3 dir = transform.position - Camera.main.transform.position;
        //transform.forward = dir;
        Debug.Log(_actual.VFX.PickUpCanvasPixelsAbove);

        Vector3 pos = Camera.main.WorldToScreenPoint(_actual.transform.position) + Vector3.up * _actual.VFX.PickUpCanvasPixelsAbove;
        pos.z = 0;
        _rectTransform.anchoredPosition3D = pos;
    }

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        _rectTransform = GetComponent<RectTransform>();
    }



    public void NewAttachment(Attachment attachment)
    {
        if (!attachment || _actual == attachment) return;

        _debug.WarningLog("NewAttachment!");
        _actual = attachment; 
        RemoveStats();

        AttachmentData data = GetData(attachment);
        myTypeText.text = data.type.ToString();
        nameText.text = data.name;
      
        //foreach (StatNames key in data.stats.Keys.Where(x => data.stats[x] != 0))
        //{
        //    View_SliderAttachment newSlider = Instantiate(sliderTemplate, Panel.transform).GetComponent<View_SliderAttachment>();
        //    View_SliderAttachment.Add(newSlider);         
        //    newSlider.SetSliderValue(key.ToString(), data.stats[key]);
        //}
    }

  
    void RemoveStats()
    {
        foreach (View_SliderAttachment item in View_SliderAttachment) 
            Destroy(item.gameObject);
        
        View_SliderAttachment.Clear();
    }

    AttachmentData GetData(Attachment attachment)
    {
        AttachmentData data = new AttachmentData();

        data.type = attachment.MyType; data.name = attachment.name;

        data.stats = new Dictionary<StatNames, int>();

        foreach (StatNames key in Enum.GetValues(typeof(StatNames)))
            if (attachment.Attachment_stats.TryGetValue(key, out var x))
                data.stats.Add(key, x.value);

        return data;
    }

   
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
