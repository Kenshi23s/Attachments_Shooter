using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Attachment
{
    [SerializeField]BaseBulltet bulletPrefab;

    private void Awake()
    {
        _myType = AttachmentType.Magazine;
    }
}


