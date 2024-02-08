using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachineBase<TState> : MonoBehaviour where TState : State
{
    [SerializeField] protected List<TState> AllStates;

    protected TState CurrentState { get; private set; }

    public void SwitchState(TState stateToSwitch)
    {
        if (CurrentState != null)
            CurrentState.Exit();
        if(stateToSwitch == null)
            return;
        CurrentState = stateToSwitch;
        CurrentState.Enter();
        OnStateEnter(stateToSwitch);

    }

    protected virtual void OnStateEnter(TState state)
    {
    }
}
