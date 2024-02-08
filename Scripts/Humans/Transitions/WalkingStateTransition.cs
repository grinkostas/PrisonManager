using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkingStateTransition : StateTransition<State>
{
    [SerializeField] private RunState _runState;
    
    protected override void OnTransitionEnable()
    {
        _runState.Escaped += Transit;
    }

    protected override void OnTransitionDisable()
    {
        _runState.Escaped -= Transit;
    }
}
