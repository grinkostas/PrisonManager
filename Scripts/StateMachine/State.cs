using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    [SerializeField] private bool _changeEnable = true;
    [SerializeField] protected bool DebugMode = false;
    public UnityAction OnStateEnter;
    public UnityAction OnStateExit;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void Enter()
    {
        if(DebugMode)
            Debug.Log($"{gameObject.name} Entered State");
        
        if(_changeEnable)
            enabled = true;
        
        OnEnter();
        if(DebugMode)
            Debug.Log($"{gameObject.name} Invoke event");
        OnStateEnter?.Invoke();
    }

    public void Exit()
    {
        if(enabled == false) return;
        OnStateExit?.Invoke();
        OnExit();
        if(_changeEnable)
            enabled = false;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
}
