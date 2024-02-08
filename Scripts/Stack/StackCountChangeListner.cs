using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public abstract class StackCountChangeListner : MonoBehaviour
{
    [SerializeField] private StackProvider _stackProvider;

    protected IStack Stack => _stackProvider.Interface;
    private void OnEnable()
    {
        _stackProvider.Interface.CountChanged += OnStackCountChanged;
        OnStackCountChanged(_stackProvider.Interface.ItemsCount);
    }
    
    private void OnDisable()
    {
        _stackProvider.Interface.CountChanged += OnStackCountChanged;
    }

    protected abstract void OnStackCountChanged(int count);
}
