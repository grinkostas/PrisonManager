using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public class DayPhaseChanger : MonoBehaviour
{
    [SerializeField] private List<DayPhaseData> _phasesOrder;

    [Inject] private Timer _timer;

    private int _currentPhaseIndex = -1;

    private Timer.TimerDelay _currentDelay = null;

    public float TimeToNextChange => _currentDelay != null ? _currentDelay.Duration - _currentDelay.WaitedTime : _phasesOrder[0].Duration;
    public DayPhaseData CurrentPhase { get; private set; }
    public UnityAction<DayPhase> PhaseChanged;

    private void Awake()
    {
        CurrentPhase = _phasesOrder[0];
    }

    private void Start()
    {
        NextPhase();
    }

    [Button("Next Phase")]
    public void NextPhase()
    {
        _currentPhaseIndex = GetNextPhaseIndex();
        var phase = _phasesOrder[_currentPhaseIndex];
        CurrentPhase = phase;
        ChangePhase(phase);
    }
    
    private void ChangePhase(DayPhaseData phase)
    {
        if(_currentDelay != null)
            _currentDelay.Kill();
        _currentDelay = _timer.ExecuteWithDelay(NextPhase, phase.Duration);
        PhaseChanged?.Invoke(phase.Phase);
    }

    private int GetNextPhaseIndex()
    {
        int currentIndex = _currentPhaseIndex;
        currentIndex++;
        if (currentIndex >= _phasesOrder.Count)
            currentIndex = 0;
        return currentIndex;
    }

    public DayPhaseData GetNextPhase()
    {
        return _phasesOrder[GetNextPhaseIndex()];
    }
    
    
}
