using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkNode<T>
{
    public T value;
    public LinkNode<T> next;
}
public class LinkedList<T> : IEnumerable
{
    public LinkNode<T> first;
    public LinkNode<T> last;

    int _count;
    public int Count
    {
        get
        {
            return _count;
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                throw new System.Exception("Index out");

            LinkNode<T> current = first;

            for (int i = 0; i < index; i++)
            {
                Debug.Log("PASO AL SIGUIENTE, FOR");
                current = current.next;
            }

            return current.value;
        }

        set
        {
            if (index < 0 || index >= _count)
                throw new System.Exception("Index out");

            LinkNode<T> current = first;

            for (int i = 0; i < index; i++)
            {
                current = current.next;
            }

            current.value = value;
        }
    }

    public void Add(T value)
    {
        var node = new LinkNode<T>();
        node.value = value;

        if(first != null)
        {
            last.next = node;
            last = node;
        }
        else
        {
            first = node;
            last = node;
        }

        _count++;
    }

    public void Remove(T value) 
    {
        LinkNode<T> nextNode = first.next;
        LinkNode<T> current = first;
        while (nextNode != null)
        {
            if (nextNode.value.GetHashCode() == value.GetHashCode())
            {
                current.next = nextNode.next;           
                break;
            }
            nextNode=nextNode.next;
            current = current.next;
        }
    }

    public void RemoveAtIndex(int index) => Remove(this[index]);

    public IEnumerator GetEnumerator()
    {
        LinkNode<T> current = first;

        while (current != null)
        {
            yield return current.value; //ESPERA....

            Debug.Log("PASO AL SIGUIENTE, FOREACH");
            current = current.next;
        }
    }
}
