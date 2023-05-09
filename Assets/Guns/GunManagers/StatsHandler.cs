using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using static Attachment;

[DisallowMultipleComponent]
public class StatsHandler : MonoBehaviour
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
        BulletSpeed

    }
    //quien deberia hacer el cambio de estadistica, el arma o el accesorio?(lo hace el arma, a partir de que el accesorio pase sus stats)
    //el arma deberia tener referencia de todos sus accesorios o no es de gran importancia?
    //(si, deberia para saber donde posicionarlos)

    public Dictionary<StatNames, int> myGunStats => _myGunStats;
    [SerializeField,SerializedDictionary("Stat", "Value")]
    SerializedDictionary<StatNames, int> _myGunStats;

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

    }
    

    
    /// <summary>
    /// este metodo cambia las estadisticas del arma, se le debe pasar un diccionario de "Stat names" como key y q el value sea un int.
    /// el booleano determina si se le restan stat(false) o se le suman(true)
    /// </summary>
    /// <param name="NewStats"></param>
    /// <param name="_isBeingAttached"></param>
    public void ChangeStats(Dictionary<StatNames, StatChangeParams> NewStats, bool _isBeingAttached)
    {       
        int x = _isBeingAttached ? 1 : -1;

        foreach (StatNames actualKey in NewStats.Keys)
        {
            if (_myGunStats.ContainsKey(actualKey))
            {
                _myGunStats[actualKey] = Mathf.Clamp(_myGunStats[actualKey] + (NewStats[actualKey].value * x), 0, 100);          
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


