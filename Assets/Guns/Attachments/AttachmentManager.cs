using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using static Attachment;
[System.Serializable]
public class AttachmentManager
{

    GunFather gun;

    //si no existe una posicion para el accesorio, es imposible colocarlo
    [SerializeField,SerializedDictionary("Attachment Type","Position In Gun")]
    SerializedDictionary<AttachmentType, Transform> _attachmentPos = new SerializedDictionary<AttachmentType, Transform>();

    //[SerializeField, SerializedDictionary("Type,Class")]
    SerializedDictionary<AttachmentType, Attachment> _activeAttachments = new SerializedDictionary<AttachmentType, Attachment>();

    public AttachmentManager(GunFather gun)
    {
        this.gun = gun;
      
    }

    public void AddAttachment(AttachmentType key, Attachment value)
    {
        if (_attachmentPos.ContainsKey(key))
        {
            if (!_activeAttachments.ContainsKey(key))
            {
                _activeAttachments.Add(key, value);
                _activeAttachments[key].Attach(gun, gun.transform, _attachmentPos[key].position);

            }
            else
            {
                ReplaceAttachment(key, value);
            }
        }
       
    }

    void ReplaceAttachment(AttachmentType key, Attachment value)
    {
        _activeAttachments[key].UnAttach();
        _activeAttachments.Remove(key);
        AddAttachment(key, value);
    }


}
