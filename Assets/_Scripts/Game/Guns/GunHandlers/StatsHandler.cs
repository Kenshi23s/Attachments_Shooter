using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using System.Linq;
using static Attachment;

[DisallowMultipleComponent]
public class StatsHandler : MonoBehaviour
{
    //clase que maneja todas las estadisticas del arma
    
    public enum StatNames
    {
       
        Range,
        Handling,                
        VerticalRecoil,
        HorizontalRecoil,
        Spread

    }
    //quien deberia hacer el cambio de estadistica, el arma o el accesorio?(lo hace el arma, a partir de que el accesorio pase sus stats)
    //el arma deberia tener referencia de todos sus accesorios o no es de gran importancia?
    //(si, deberia para saber donde posicionarlos)
    public const float MaxStatValue = 100;
    public  const float MinStatValue = 0;

    public float GetStat(StatNames key)
    {
        return Mathf.Clamp(_myGunStats[key], MinStatValue, MaxStatValue);
    }

    public Dictionary<StatNames, int> statDictionary => _myGunStats;
    [SerializeField,SerializedDictionary("Stat", "Value")]
    SerializedDictionary<StatNames, int> _myGunStats;

    private void Awake()
    {
        string statName = "";
        foreach (StatNames item in Enum.GetValues(typeof(StatNames)))
        {
            if (!_myGunStats.ContainsKey(item))
            {
                _myGunStats.Add(item, 1);
                statName += item.ToString() + ", ";
            }
        }
        Debug.LogWarning($"El arma no contenia la/s estadistica {statName} asi que la/s cree y le asigne valor = 1");
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
                _myGunStats[actualKey] = _myGunStats[actualKey] + NewStats[actualKey].value * x;      
        }        
    }

    //statChangeParameters no deberia clampear nada (StatChangeParams) .. ahora no se usa, queda como borrador
    #region ElNuevoChangeStat
    /// <summary>
    /// Le pasas las stats del baseGun y las stats de los attachments y te devuelve las stats nuevas
    /// </summary>
    /// <param name="myGunStat"></param>
    /// <param name="MyAtachments"></param>
    /// <returns></returns>
    public SerializedDictionary<StatNames, int> RecalculateStats(SerializedDictionary<StatNames, int> myGunStat, List<SerializedDictionary<StatNames,int>> MyAtachments)
    {
        SerializedDictionary<StatNames, int> newStats = myGunStat;

        foreach (var item in myGunStat)
        {
            newStats[item.Key] = Mathf.Clamp(CalculateSingleStat( MyAtachments.Where(x => x.ContainsKey(item.Key)).Select(z => z[item.Key]).ToList(), myGunStat[item.Key]),0,100);
        }

        return newStats;

    }

    //debes pasarle los valores de los atachments y el basegun para que te lo recalcule (es algo interno de recalculate stats)
    int CalculateSingleStat(List<int> atachValues, int mygunValue)
    {
        for (int i = 0; i < atachValues.Count; i++)
        {
            mygunValue += atachValues[i];
        }

        return Mathf.Clamp(mygunValue,0,100);
    }
    #endregion
}


