using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ActionPlaceExtention : MonoBehaviour
{
    [SerializeField] private ActionPlace _actionPlaceToExtend;
    private void OnEnable()
    {
        _actionPlaceToExtend.Used += OnUsePlace;
        _actionPlaceToExtend.Kicked += OnKick;
    }

    private void OnDisable()
    {
        _actionPlaceToExtend.Used -= OnUsePlace;
        _actionPlaceToExtend.Kicked -= OnKick;
    }

    protected abstract void OnUsePlace(Prisoner prisoner);
    protected abstract void OnKick(Prisoner prisoner);
}
