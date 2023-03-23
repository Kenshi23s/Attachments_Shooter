using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Attachment
{
    SpriteRenderer _sightDot;
    public SpriteRenderer sightDot => _sightDot;
    private void Awake()
    {
        _myType = AttachmentType.Sight;
    }
}
