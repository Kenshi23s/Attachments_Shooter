using System.Collections.Generic;
using UnityEngine;
using static Attachment;

public class AttachmentManager
{

    GunFather gun;

    Dictionary<AttachmentType, Vector3> _attachmentPos = new Dictionary<AttachmentType, Vector3>();

    Dictionary<AttachmentType, Attachment> _activeAttachments = new Dictionary<AttachmentType, Attachment>();

    public AttachmentManager(GunFather gun, Dictionary<AttachmentType, Vector3> _attachmentPos)
    {
        this.gun = gun;
        this._attachmentPos = _attachmentPos;
    }

    public void AddAttachment(AttachmentType key, Attachment value)
    {
        if (!_activeAttachments.ContainsKey(key))
        {
            _activeAttachments.Add(key, value);
            _activeAttachments[key].Attach(gun, gun.transform, _attachmentPos[key]);

        }
        else
        {
            ReplaceAttachment(key, value);
        }
    }

    void ReplaceAttachment(AttachmentType key, Attachment value)
    {
        _activeAttachments[key].UnAttach();
        _activeAttachments.Remove(key);
        AddAttachment(key, value);
    }


}
