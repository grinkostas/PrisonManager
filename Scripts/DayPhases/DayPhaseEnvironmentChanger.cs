using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK;

public class DayPhaseEnvironmentChanger : DayPhaseListner
{
    [SerializeField] private MaterialFade _materialFade;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private List<EnvironmentSetting> _environmentSettings;

    protected override void OnPhaseChanged(DayPhase phase)
    {
        EnvironmentSetting setting = _environmentSettings.Find(x => x.Phase == phase);
        
        if(setting.ChangeMaterial)
            _materialFade.Fade(material => RenderSettings.skybox = material, RenderSettings.skybox, setting.SkyMaterial, _fadeDuration);
        
        Animations.ColorFade(this, RenderSettings.ambientSkyColor, setting.SkyColor, 
            color => RenderSettings.ambientSkyColor = color, _fadeDuration);
        
        Animations.ColorFade(this, RenderSettings.ambientEquatorColor, setting.EquatorColor, 
            color => RenderSettings.ambientEquatorColor = color, _fadeDuration);
        
        Animations.ColorFade(this, RenderSettings.ambientGroundColor, setting.GroundColor, 
            color => RenderSettings.ambientGroundColor = color, _fadeDuration);
    }
    
    [System.Serializable]
    private class EnvironmentSetting
    {
        public DayPhase Phase;
        public bool ChangeMaterial;
        [ShowIf(nameof(ChangeMaterial))] public Material SkyMaterial;
        [ColorUsage(true, true)]
        public Color SkyColor;
        [ColorUsage(true, true)]
        public Color EquatorColor;
        [ColorUsage(true, true)]
        public Color GroundColor;
    }

}
