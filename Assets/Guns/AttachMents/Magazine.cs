using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Attachment
{
    [SerializeField]BaseBulltet _bulletPrefab;
    
    public string bulletKey=>_bulletKey;
    private string _bulletKey;
    private void Awake()
    {
        
    }
    private void Start()
    {
        if (_bulletKey==default)
        {

            _bulletKey = Random.Range(0, 5000).ToString();
        }
        Bullet_Manager.instance.CreateBullet(_bulletKey, _bulletPrefab);
    }
}


