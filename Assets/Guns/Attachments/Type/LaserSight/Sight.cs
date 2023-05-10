using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Attachment
{
    SpriteRenderer _sightDot;
    public SpriteRenderer sightDot => _sightDot;

    protected override void Initialize()
    {
        _myType = AttachmentType.Sight;
    }

}
