using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Attachment
{
    [SerializeField]BaseBulltet _bulletPrefab;
    
    public int bulletKey=>_bulletKey;
    private int _bulletKey;

    private void Awake()
    {
        _myType = AttachmentType.Magazine;
    }
    private void Start() => _bulletKey = Bullet_Manager.instance.CreateBulletPool(_bulletPrefab);

}


