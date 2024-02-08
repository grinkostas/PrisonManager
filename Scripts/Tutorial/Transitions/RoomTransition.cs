using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomTransition : StateTransition
{
    [SerializeField] private Room _room;
    
    protected override void OnTransitionEnable()
    {
        foreach (var bed in _room.Beds)
        {
            bed.OnPopulate += OnPopulate;
        }
    }

    protected override void OnTransitionDisable()
    {
        foreach (var bed in _room.Beds)
        {
            bed.OnPopulate -= OnPopulate;
        }
    }

    private void OnPopulate()
    {
        Transit();
    }
}
