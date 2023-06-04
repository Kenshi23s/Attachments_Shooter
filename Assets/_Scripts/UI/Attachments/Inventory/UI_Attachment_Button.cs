using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(DebugableObject))]
public class UI_Attachment_Button : MonoBehaviour
{
    Button button;
   [SerializeField] TMP_Text Buttontext;
    DebugableObject _debug;
   public Attachment owner { get; private set; }
    Gun displayGun;

    Action<UI_Attachment_Button> _onChange;

    void Awake()
    {
        button = GetComponent<Button>();
        _debug = GetComponent<DebugableObject>();
    } 

    public UI_Attachment_Button AssignAttachment(Attachment x,Gun y,Action<UI_Attachment_Button> onChange)
    {
        button.onClick.RemoveAllListeners();
        owner = x;
        displayGun = y;
        _onChange = onChange;
     
        Buttontext.text = owner.gameObject.name;
        gameObject.name = "[Button]" + Buttontext.text;

        if (x.isAttached)
            button.onClick.AddListener(SaveAttachment);
        else
            button.onClick.AddListener(EquipAttachment);
        
        return this;        
    }

    void SaveAttachment()
    {
        _debug.Log($"Guardo el accesorio {owner}");
        AttachmentManager.instance.Inventory_SaveAttachment(owner);
        _onChange?.Invoke(this);
    }

    void EquipAttachment()
    {
   
        string debug =  $"Equipo el accesorio {owner} a {displayGun} ";
        if (displayGun.attachmentHandler.activeAttachments.ContainsKey(owner.myType))
        {
             var aux = displayGun.attachmentHandler.activeAttachments[owner.myType];
            displayGun.attachmentHandler.RemoveAttachment(aux.myType);
            debug += $",PERO ANTES desconecto el accesorio {aux} (la cual de mi mismo tipo) de el arma";
        }
        _debug.Log(debug);

        displayGun.attachmentHandler.AddAttachment(AttachmentManager.instance.RemoveFromInventory(owner));

        _onChange?.Invoke(this);
    }
}
