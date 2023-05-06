using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Attachment;

public class AttachmentManager 
{
    //lo debe crear el gunManager?
    Dictionary<AttachmentType, Dictionary<int,Attachment>> attachmentsInventory;

    public void SaveAttachment(GunFather gun,Attachment value)
    {
        int key = value.GetHashCode();
        if (attachmentsInventory[value.myType].ContainsKey(key)) return;
        gun.attachmentHandler.RemoveAttachment(value.myType); value.Dettach();
        attachmentsInventory[value.myType].Add(key,value);
    }
                                                 //key diccionario 1(enum Attachment) 
                                                 //key diccionario 2(HashCode)
    public bool GetAttachment(out Attachment x,Tuple<AttachmentType, int> keys)
    {
        x = default;
        
        if (!attachmentsInventory.ContainsKey(keys.Item1)) return false;
        if (!attachmentsInventory[keys.Item1].ContainsKey(keys.Item2)) return false;
     
        x = attachmentsInventory[keys.Item1][keys.Item2]; 

        attachmentsInventory[keys.Item1].Remove(keys.Item2); return true;

    }

}
