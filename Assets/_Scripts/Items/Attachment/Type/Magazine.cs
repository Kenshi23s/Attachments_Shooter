using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Attachment
{
    [SerializeField]BaseBulltet _bulletPrefab;
    
    public int bulletKey=>_bulletKey;
    private int _bulletKey;

   

    protected override void Comunicate()
    {
        
    }
    protected override void Initialize()
    {
        MyType = AttachmentType.Magazine;
    }
}


