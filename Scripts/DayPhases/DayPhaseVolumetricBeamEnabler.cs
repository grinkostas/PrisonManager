using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using VLB;

public class DayPhaseVolumetricBeamEnabler : DayPhaseListner
{
    [SerializeField] private DayPhase _enableDayPhase;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private VolumetricLightBeam _volumetricLightBeam;

    private float _startIntensity;
    
    private void Awake()
    {
        _startIntensity = _volumetricLightBeam.intensityInside;
        _volumetricLightBeam.intensityInside = 0;
        _volumetricLightBeam.intensityOutside = 0;
    }
    
    protected override void OnPhaseChanged(DayPhase phase)
    {
        float startValue = 0;
        float endValue = _startIntensity;
        if (phase != _enableDayPhase)
        {
            startValue = _volumetricLightBeam.intensityGlobal;
            endValue = 0f;
        }
        Animations.ValueFade(this, startValue, endValue, value=> _volumetricLightBeam.intensityInside = value, _fadeDuration);
        Animations.ValueFade(this, startValue, endValue, value=> _volumetricLightBeam.intensityOutside = value, _fadeDuration);
    }
}
