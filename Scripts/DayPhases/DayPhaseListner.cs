using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public abstract class DayPhaseListner : MonoBehaviour
{
    [Inject] private DayPhaseChanger _dayPhaseChanger;

    private void OnEnable()
    {
        _dayPhaseChanger.PhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        _dayPhaseChanger.PhaseChanged -= OnPhaseChanged;
    }

    protected abstract void OnPhaseChanged(DayPhase phase);
}
