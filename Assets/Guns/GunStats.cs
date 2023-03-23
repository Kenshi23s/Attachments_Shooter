using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using static UnityEditor.Progress;

[System.Serializable]
public class GunStats
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
        BulletRadius

    }
    //quien deberia hacer el cambio de estadistica, el arma o el accesorio?(lo hace el arma, a partir de que el accesorio pase sus stats)
    //el arma deberia tener referencia de todos sus accesorios o no es de gran importancia?
    //(si, deberia para saber donde posicionarlos)

    public Dictionary<StatNames, int> myGunStats => _myGunStats;
    [SerializeField,SerializedDictionary("Stat", "Value")]
    SerializedDictionary<StatNames, int> _myGunStats;


    //[SerializeField] Transform defaultShootPos;
    //public Transform shootPos => shootPos;
    //[SerializeField]Transform _shootPos;

    //[SerializeField]
    //public List<StatNames> myGunStatsList = new List<StatNames>();

    public void Initialize()
    {
        string statsname = ""; 
        foreach (StatNames item in Enum.GetValues(typeof(StatNames)))
        {
           
            if (!_myGunStats.ContainsKey(item))
            {
                _myGunStats.Add(item, 1);

                statsname += item.ToString()+ ", ";
            }

           
        }
        Debug.LogWarning($"El arma no contenia la/s estadistica {statsname} asi que la/s cree y le asigne valor = 1");
        //temporal, solo para testeo
        //foreach (StatNames stat in myGunStatsList)
        //{
        //    myGunStats[stat] = Random.Range(0, 101);

        //}
    }
    

    
    /// <summary>
    /// este metodo cambia las estadisticas del arma, se le debe pasar un diccionario de "Stat names" como key y q el value sea un int.
    /// el booleano determina si se le restan stat(false) o se le suman(true)
    /// </summary>
    /// <param name="NewStats"></param>
    /// <param name="_isBeingAttached"></param>
    public void ChangeStats(Dictionary<StatNames, int> NewStats, bool _isBeingAttached)
    {       
        int x = _isBeingAttached ? 1 : -1;

        foreach (StatNames actualKey in NewStats.Keys)
        {
            if (_myGunStats.ContainsKey(actualKey))
            {
                _myGunStats[actualKey] = Mathf.Clamp(_myGunStats[actualKey] + (NewStats[actualKey] * x), 0, 100);          
            }
        } 

        
    }

    //void CheckShootPos()
    //{
    //    if (shootPos==null)
    //    {
    //        _shootPos = defaultShootPos;
    //    }
    //}
    //public void ChangeShootPos(Transform newShootPos)
    //{
    //    if (newShootPos!=null)
    //    {
    //        _shootPos=newShootPos;
    //    }
    //    else
    //    {
    //        _shootPos = defaultShootPos;
    //    }
    //}
}

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


