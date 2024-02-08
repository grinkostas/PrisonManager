using UnityEngine;
using Zenject;
using System.Collections;
using System.Collections.Generic;

public class GrabTransition : StateTransition
{
    [SerializeField] private StackProvider _stack;
    
    protected override void OnTransitionEnable()
    {
        _stack.Interface.CountChanged += OnAddedItem;
    }

    protected override void OnTransitionDisable()
    {
        _stack.Interface.CountChanged -= OnAddedItem;
    }

    private void OnAddedItem(int count)
    {
        if(count > 0)
            Transit();
    }
}
