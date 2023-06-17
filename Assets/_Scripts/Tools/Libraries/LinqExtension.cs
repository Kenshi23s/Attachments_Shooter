using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LinqExtension
{
    public static T Minimum<T>(this IEnumerable<T> col,Func<T,float> GetValue)
    {
        float minimum = float.MaxValue;
        T returnItem = default;
        foreach (var item in col)
        {
            float newValue = GetValue(item);
            if (minimum > newValue)
            {
                minimum = newValue;
                returnItem = item;
            }
           
        }
        return returnItem;
       
    }

    public static T Maximum<T>(this IEnumerable<T> col, Func<T,float> GetValue)
    {
        float maximum = float.MinValue;
        T returnItem = default;
        foreach (var item in col)
        {
            float newValue = GetValue(item);
            if (newValue> maximum)
            {
                maximum = newValue;
                returnItem = item;
            }

        }
        return returnItem;

    }
}
