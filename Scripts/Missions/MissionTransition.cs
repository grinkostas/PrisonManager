using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Zenject;

public abstract class MissionTransition : StateTransition<MissionState>
{
    protected override void OnTransitionEnable()
    {
        if (TryComplete(out float progress))
        {
            CurrentState.ProgressChanged?.Invoke(progress);
            Complete();
            return;
        }
        OnMissionEnter();
    }

    protected abstract void OnMissionEnter();

    protected void Complete()
    {
        CurrentState.Complete();
        Transit();
    }
    
    protected bool TryComplete(out float progress)
    {
        progress = GetProgress();
        if (CurrentState.Completed)
        {
            Complete();
            return true;
        }
        
        return AdditionSkipCondition();
    }

    protected abstract float GetProgress();

    protected virtual bool AdditionSkipCondition()
    {
        return false;
    }


}
