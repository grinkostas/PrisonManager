using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : StateMachineBase<State>
{
    [SerializeField] private State _startState;

    private void Start()
    {
        AllStates.ForEach(x=> x.Exit());
        SwitchState(_startState);
        OnStart();
    }

    protected virtual void OnStart()
    {
    }


}
