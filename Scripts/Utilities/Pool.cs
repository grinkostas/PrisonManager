using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Pool<T>
{
    private List<T> _poolSource;
    private List<T> _currentPool;

    public Pool(List<T> poolSource)
    {
        _poolSource = poolSource;
        ResetPool();
    }

    private void ResetPool()
    {
        _currentPool = new List<T>(_poolSource);
    }

    public T RandomFromPool(Predicate<T> predicate)
    {
        if(_currentPool.Count == 0)
            ResetPool();
        
        var items = _currentPool.FindAll(predicate);
        if (items.Count == 0)
            items = _poolSource.FindAll(predicate);
        var item = items[Random.Range(0, items.Count)];
        _currentPool.Remove(item);
        return item;
    }

}
