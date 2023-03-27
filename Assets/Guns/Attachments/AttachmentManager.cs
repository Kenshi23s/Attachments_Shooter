using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using static Attachment;
[System.Serializable]
public class AttachmentManager
{

    GunFather gun;
    public void AssignGun(GunFather gun)
    {
        this.gun = gun;
    }

    //si no existe una posicion para el accesorio, es imposible colocarlo
    [SerializeField,SerializedDictionary("Attachment Type","Position In Gun")]
    SerializedDictionary<AttachmentType, Transform> _attachmentPos = new SerializedDictionary<AttachmentType, Transform>();

    //[SerializeField, SerializedDictionary("Type,Class")]
    SerializedDictionary<AttachmentType, Attachment> _activeAttachments = new SerializedDictionary<AttachmentType, Attachment>();
    public SerializedDictionary<AttachmentType, Attachment> activeAttachments => _activeAttachments;

    [SerializeField,SerializedDictionary("Attachment Type","Position In Gun")]
    SerializedDictionary<AttachmentType, Attachment> _DefaultAttachMent = new SerializedDictionary<AttachmentType, Attachment>();

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
                Debug.Log($"conecte el attachment de tipo{key} a el arma");
                return;
            }
            else
            {
                ReplaceAttachment(key, value);
            }
        }
        Debug.Log($"NO conecte el attachment de tipo{key} a el arma, ya que no tengo una posicion para darle");

    }

    void ReplaceAttachment(AttachmentType key, Attachment value)
    {
        _activeAttachments[key].UnAttach();
        _activeAttachments.Remove(key);
        AddAttachment(key, value);
    }

    public void RemoveAttachment(AttachmentType key)
    {
        if (_activeAttachments.ContainsKey(key))
        {
            _activeAttachments.Remove(key);

            if (_DefaultAttachMent.ContainsKey(key))
            {
                AddAttachment(key, _DefaultAttachMent[key]);
            }
        }
    }

}
