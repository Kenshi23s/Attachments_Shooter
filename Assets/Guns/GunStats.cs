using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GunStats : MonoBehaviour
{
    public enum StatNames
    {
        AimZoom,
        Range,
        Handling,      
        Stability,
        ReloadSpeed,
        MaxMagazine,
        AmooPerShoot,
        BulletRadius,

    }
    //quien deberia hacer el cambio de estadistica, el arma o el accesorio?
    //el arma deberia tener referencia de todos sus accesorios o no es de gran importancia?
    //(si, deberia para saber donde posicionarlos)
    public Dictionary<StatNames, float> myGunStats => _myGunStats;
    Dictionary<StatNames,float> _myGunStats = new Dictionary<StatNames,float>();

    [SerializeField] Transform defaultShootPos;
    public Transform _shootPos => shootPos;
    Transform shootPos;

    //[Header("GunStats")]
    //[SerializeField, Range(1, 100)] float _aimingZoom;
    //[SerializeField, Range(1, 100)] float _range;

    //[SerializeField, Range(1, 100)] float _handling;
    //[SerializeField, Range(1, 100)] float _stability;

    //[SerializeField, Range(1, 100)] float _reloadSpeed;

    //[SerializeField, Range(1, 100)] float _bulletMagnetism;

    //[SerializeField] int _maxMagazineAmmo;
    //#region Stat Getters    

    //public float aimingZoom => _aimingZoom;
    //public float range => _range;

    //public float handling => _handling;
    //public float stability => _stability;

    //public float reloadSpeed => _reloadSpeed;
    //public int maxMagazineAmmo => _maxMagazineAmmo;

    //#endregion

    //#region StatMethods
    //private void AddAimZoom(float value) => _aimingZoom += Mathf.Clamp(value, 1, 100);
    //private void AddRange(float value) => _range += Mathf.Clamp(value, 1, value);

    //private void AddReloadSpeed(float value) => _reloadSpeed += Mathf.Clamp(value, 1, 100);
    //private void AddHandling(float value) => _handling += Mathf.Clamp(value, 1, 100);

    //private void AddStability(float value) => _stability += Mathf.Clamp(value, 1, 100);
    //private void AddMaxMagCapacity(float value) => _stability += Mathf.Clamp(value, 1, 100);

    public void ChangeStats(AttachmentStats NewStats, bool _isBeingAttached)
    {       

        int x = _isBeingAttached ? 1 : -1;

        foreach (StatNames actualKey in NewStats._Stats.Keys)
        {
            if (_myGunStats.ContainsKey(actualKey))
            {             
                _myGunStats[actualKey] = Mathf.Clamp(_myGunStats[actualKey] + (NewStats._Stats[actualKey] * x),0,100);            
            }
        } 
    }
}
//AddAimZoom(NewStats.aimingZoom * x);
//AddRange(NewStats._range * x);

//AddReloadSpeed(NewStats.reloadSpeed * x);
//AddHandling(NewStats.handling * x);

//AddStability(NewStats.stability * x);
//AddMaxMagCapacity(NewStats.maxMagazineAmmo * x)
//#endregion

//private void ChangeShootPos(Transform newShootpos)
//{
//    if (newShootpos != null)
//    {
//        shootPos = newShootpos;
//    }
//    else
//    {
//        shootPos = defaultShootPos;
//    }
//}


