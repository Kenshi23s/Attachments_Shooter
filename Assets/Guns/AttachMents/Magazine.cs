using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Attachment
{
    [SerializeField]GameObject bulletPrefab;
    private void Awake()
    {
        myType = AttachmentType.Magazine;
    }
}


