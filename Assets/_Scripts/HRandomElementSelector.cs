using System;
using System.Collections.Generic;
using System.Linq;

public class HRandomElementSelector<T>
{
    private readonly Random _random;
    private readonly List<T> _history;
    private readonly int _historySize;
    private List<T> _eligibleElements;

    public HRandomElementSelector(T[] array, int historySize)
    {
        _random = new Random();
        _history = new List<T>();
        _historySize = historySize;

        // Initialize the eligible elements using all the elements in the array
        _eligibleElements = array.ToList();
    }

    public T SelectRandomElement()
    {
        //if (_eligibleElements.Count < _historySize)
        //{
        //    _history.Clear();
        //    _eligibleElements = _eligibleElements.Union(_history).ToList();
        //}
        
        // Conseguir elemento elegible al azar
        int index = _random.Next(_eligibleElements.Count);
        T element = _eligibleElements[index];

        // Agregarlo al historial y removerlo de la lista de elementos elegibles
        _history.Add(element);
        _eligibleElements.Remove(element);

        // Si el historial sobrepasa su tamaño, remover el primer elemento y devolverlo a la lista de elementos elegibles
        if (_history.Count > _historySize)
        {
            T aux = _history[0];
            _history.RemoveAt(0);
            _eligibleElements.Add(aux);
        }

        return element;
    }
}
