using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : Attachment
{
    protected override void Initialize()
    {
        _myType = AttachmentType.Stock;
    }
    protected override void Comunicate()
    {
        throw new System.NotImplementedException();
    }


}
