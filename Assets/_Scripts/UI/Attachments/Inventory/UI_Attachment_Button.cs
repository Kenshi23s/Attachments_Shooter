using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(DebugableObject))]
public class UI_Attachment_Button : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    Button _button;
    [SerializeField] TMP_Text Buttontext;
    DebugableObject _debug;
    public Attachment owner { get; private set; }
    Gun displayGun;
    Transform panel;
    List<View_SliderAttachment> sliders = new List<View_SliderAttachment>();
    
    //Action<UI_Attachment_Button> _onChange;

    void Awake()
    {
        _button = GetComponent<Button>();
        _debug = GetComponent<DebugableObject>();
    }

    //public UI_Attachment_Button AssignAttachment(Attachment newOwner, Gun newDisplayGun, Action<UI_Attachment_Button> onChange)
    //{
    //    _button.onClick.RemoveAllListeners();
    //    owner = newOwner;
    //    displayGun = newDisplayGun;
    //    _onChange = onChange;

    //    Buttontext.text = owner.gameObject.name;

    //    gameObject.name = "[Button]" + Buttontext.text;


    //    if (newOwner.isAttached)
    //    {
    //        _button.onClick.AddListener(SaveAttachment);
    //        _button.image.color = Color.green;
    //    }
    //    else
    //    {
    //        _button.image.color = Color.green;
    //        _button.onClick.AddListener(EquipAttachment);
    //    }


    //    return this;
    //}

    public UI_Attachment_Button AssignAttachment(Attachment newOwner, Gun newDisplayGun)
    {
        _button.onClick.RemoveAllListeners();
        owner = newOwner; displayGun = newDisplayGun;

        if (displayGun == null || owner == null) 
        { 
            Debug.LogError($"El arma es {newDisplayGun} y accesorio es {newOwner}, no puedo Inicializar el boton"); 
            return this; 
        }

        Buttontext.text = owner.gameObject.name; gameObject.name = "[Button]" + Buttontext.text;

        if (newOwner.isAttached)
        {
            _button.onClick.AddListener(SaveAttachment);
            _button.image.color = Color.green;
        }
        else
        {
            _button.image.color = Color.white;
            _button.onClick.AddListener(EquipAttachment);
        }
        return this;
    }

    void SaveAttachment()
    {
        _debug.Log($"Guardo el accesorio {owner}");
        AttachmentManager.instance.Inventory_SaveAttachment(owner);
        //_onChange?.Invoke(this);
    }

  
   public void ShowStat(bool arg)
   {
        if (arg)
        {
            DeleteSliders();
            Buttontext.gameObject.SetActive(false);
          
            foreach (var key in owner.Attachment_stats.Keys)
            {
                ////View_SliderAttachment statSlider = Instantiate(sliderPrefab, panel);
                
                //statSlider.SetSliderValue(key.ToString(), owner.Attachment_stats[key].value);

                //sliders.Add(statSlider);
            }
        }
        else
        {
            DeleteSliders();
            Buttontext.gameObject.SetActive(true);  
            Buttontext.text = owner.gameObject.name;
        }
        
   }

    void DeleteSliders()
    {
        if (!sliders.Any()) return;
       
        foreach (var item in sliders)       
            Destroy(item.gameObject);

        sliders.Clear();
        
    }

    void EquipAttachment()
    {
        //shortcut
        var attachHandler = displayGun.attachmentHandler;

        string msg =  $"Equipo el accesorio {owner} a {displayGun} ";
        if (attachHandler.activeAttachments.ContainsKey(owner.MyType))
        {
            var aux = attachHandler.activeAttachments[owner.MyType];
            attachHandler.RemoveAttachment(aux.MyType);
            msg += $",PERO ANTES desconecto el accesorio {aux} (la cual de mi mismo tipo) de el arma";
        }
        _debug.Log(msg);

        attachHandler.AddAttachment(AttachmentManager.instance.RemoveFromInventory(owner));

        //_onChange?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
