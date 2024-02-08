using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public abstract class StateTransition : StateTransition<State>
{
}

public abstract class StateTransition<TState> : MonoBehaviour where TState : State
{
    [SerializeField] protected bool DebugMode = false;
    [SerializeField] private TState _startState;
    [SerializeField, HideIf(nameof(_lastState))] private TState _targetState;
    [SerializeField] private StateMachineBase<TState> _stateMachine;
    [SerializeField] private bool _lastState;

    protected TState CurrentState => _startState;
    protected bool TransitionAvailable { get; private set; } = false;

    private void OnEnable()
    {
        if (DebugMode)
        {
            Debug.Log($"{gameObject.name} Subscribes");
        }
        _startState.OnStateEnter += OnStartStateEnter;
        _startState.OnStateExit += OnStartStateExit;
    }

    private void OnDisable()
    {
        if (DebugMode)
        {
            Debug.Log($"{gameObject.name} Unsubscribes");
        }
        _startState.OnStateEnter -= OnStartStateEnter;
        _startState.OnStateExit -= OnStartStateExit;
    }

    private void OnStartStateEnter()
    {
        if (DebugMode)
        {
            Debug.Log($"{gameObject.name} OnStartStateEnter");
        }
        TransitionAvailable = true;
        OnTransitionEnable();
    }

    private void OnStartStateExit()
    {
        TransitionAvailable = false;
        OnTransitionDisable();
    }

    protected virtual void OnTransitionEnable()
    {
    }

    protected virtual void OnTransitionDisable()
    {
    }


    protected void Transit()
    {
        if (_lastState)
        {
            _startState.Exit();
            return;
        }

        _stateMachine.SwitchState(_targetState);
    }


}
