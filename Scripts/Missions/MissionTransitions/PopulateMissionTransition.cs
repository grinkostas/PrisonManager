using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopulateMissionTransition : MissionTransition
{
    [SerializeField] private int _prisonerToPopulate;
    [SerializeField] private List<Room> _rooms;

    private int _populated = 0;

    protected override void OnMissionEnter()
    {
        _populated = 0;
        foreach (var room in _rooms)
        {
            foreach (var bed in room.Beds)
            {
                bed.OnPopulate += OnPopulate;
            }    
        }
    }

    protected override float GetProgress()
    {
        return _populated / (float)_prisonerToPopulate;
    }

    protected override void OnTransitionDisable()
    {
        foreach (var room in _rooms)
        {
            foreach (var bed in room.Beds)
            {
                bed.OnPopulate -= OnPopulate;
            }    
        }
    }

    private void OnPopulate()
    {
        _populated++;
        CurrentState.ProgressChanged?.Invoke(GetProgress());
        if (_populated >= _prisonerToPopulate)
            Complete();
    }
}

