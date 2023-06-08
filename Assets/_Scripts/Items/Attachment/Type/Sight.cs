using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Attachment
{
    [SerializeField]SpriteRenderer _sightDot;
    public SpriteRenderer sightDot => _sightDot;

    public Transform sightPoint;

    protected override void Initialize() => _myType = AttachmentType.Sight;
   
    protected override void Comunicate() { }
}
