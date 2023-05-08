using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : Attachment
{
    private void Awake()
    {
        _myType = AttachmentType.Stock;
    }
}
