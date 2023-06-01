using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attachment_Button : MonoBehaviour
{
    Button button;
   [SerializeField] TMP_Text Buttontext;

    bool isEquipped;
    Attachment owner;

    private void Awake()
    {
        button= GetComponent<Button>();
        
    }
    public void AssignAttachment(Attachment x)
    {
        owner = x;
        Buttontext.text = owner.TESTNAME;
      
        
    }

    void SaveAttachment()
    {
        AttachmentManager.instance.Inventory_SaveAttachment(owner);
    }

    void EquipAttachment(Gun target)
    {        
        target.attachmentHandler.AddAttachment(AttachmentManager.instance.RemoveFromInventory(owner));
    }
}
