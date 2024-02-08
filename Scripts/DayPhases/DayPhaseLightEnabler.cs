using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using StaserSDK.Utilities;
using Zenject;

public class DayPhaseLightEnabler : DayPhaseListner
{
    [SerializeField] private DayPhase _enableDayPhase;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private Light _light;

    [Inject] private Timer _timer;

    private Timer.TimerDelay _currentDelay;
    private Color _startColor;

    private void Awake()
    {
        _startColor = _light.color;
    }

    protected override void OnPhaseChanged(DayPhase phase)
    {
        if (phase == _enableDayPhase)
        {
            Enable();
            return;
        }
        Disable();
    }

    private void Disable()
    {
        ChangeLightColor(Color.black);
        _currentDelay = _timer.ExecuteWithDelay(() => _light.enabled = false, _fadeDuration);
    }

    private void Enable()
    {
        if(_currentDelay != null)
            _currentDelay.Kill();
        _light.enabled = true;
        ChangeLightColor(_startColor);
    }

    private void ChangeLightColor(Color color)
    {
        Animations.ColorFade(this, _light.color, color, 
            actualizedColor => _light.color = actualizedColor, _fadeDuration);
    }
}
