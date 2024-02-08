using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropStackTransition : StateTransition
{
    [SerializeField] private StackProvider _stack;
    
    protected override void OnTransitionEnable()
    {
        _stack.Interface.CountChanged += OnCountChanged;
    }

    protected override void OnTransitionDisable()
    {
        _stack.Interface.CountChanged -= OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        if(count == 0)
            Transit();
    }
}
