using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using Zenject;

public class DayPhaseLightChanger : DayPhaseListner
{
    [SerializeField] private Light _targetLight;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private List<LightSetting> _lightSettings;
    
    protected override void OnPhaseChanged(DayPhase phase)
    {
        LightSetting lightSetting = _lightSettings.Find(x => x.Phase == phase);
        Animations.ColorFade(this, _targetLight.color, lightSetting.Color, color => _targetLight.color = color, _fadeDuration);
        Animations.ValueFade(this, _targetLight.intensity, lightSetting.Intensity, value => _targetLight.intensity = value, _fadeDuration);
    }
    
    [System.Serializable]
    private class LightSetting
    {
        public DayPhase Phase;
        public Color Color;
        public float Intensity;
    }
}
