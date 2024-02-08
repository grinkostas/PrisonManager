using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DayPhaseObjectEnabler : DayPhaseListner
{
    [SerializeField] private DayPhase _targetPhase;
    [SerializeField] private GameObject _objectToEnable;
    protected override void OnPhaseChanged(DayPhase phase)
    {
        if (_targetPhase == phase)
        {
            _objectToEnable.SetActive(true);
            return;
        }
        _objectToEnable.SetActive(false);
    }
}
