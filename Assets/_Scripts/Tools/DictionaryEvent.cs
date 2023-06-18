using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryEvent<K>
{
    public delegate void DictionaryAction();

    Dictionary<K, DictionaryAction> _eventDictionary = new Dictionary<K, DictionaryAction>();

}
