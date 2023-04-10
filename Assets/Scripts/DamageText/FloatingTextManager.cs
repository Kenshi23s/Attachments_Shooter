using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static FloatingText;

[System.Serializable]
public class FloatingTextManager
{
  
    TextPool pool;

    [SerializeField]
    FloatingTextParam _parameters;
    [SerializeField]
    FloatingText sampleFloatingText;
    public void Initialize(Transform transform) => pool.Initialize(transform, sampleFloatingText);
    
    //podriamos tener en este metodo variaciones por ej
    // si es critico o baja que salga de otro color el texto, con otra fuente, o otra proporcion
    //revisar para 6to cuatrimestre o cuando no haya tareas heavy


    //esto deberia subscribirse a el gun manager.actualgunhit<HitData>, por ahora lo dejo asi
    public void PopUpHitText(HitData data)
    {
        FloatingText t = pool.GetHolder();

        if (t != null)
        {
            pool.TurnOnHolder(t);                     //Z randomnes deberia ser Y randomnes,
            _parameters.IncreaseSortingOrder();      //pq esto ya no es un top down, ajustar luego.
            t.InitializeText(data.dmgDealt, data._impactPos, _parameters);
            
        }
    }

   

}
