using System;
using UnityEngine;

public interface IPoolableObject
{
    void EnterPool(); void ExitPool();
}
public class PoolableObject<T> : IPoolableObject
{
    public event Action onPoolEnter;
    public event Action onPoolLeave;

    Action<T, int> _poolReturn;

    protected int poolKey;
    T objectToPool;

    public void Awake(T objectToPool)
    {
        this.objectToPool = objectToPool;
    }
  
    public virtual void Initialize(Action<T, int> _poolReturn,int key) => this._poolReturn = _poolReturn;

    public void EnterPool()
    {
        _poolReturn?.Invoke(objectToPool, poolKey);
        onPoolEnter?.Invoke();
    }

    public void ExitPool() => onPoolLeave?.Invoke();
}
