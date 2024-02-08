using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using StaserSDK.Utilities;
using Zenject;

public class EmissionPulser : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private int _materialIndex = 0;
    [SerializeField] private float _pulseDelay;
    [SerializeField] private float _pulseFadeDelay;
    [SerializeField] [ColorUsage(true, true)] private Color _startColor;
    [SerializeField] [ColorUsage(true, true)] private Color _endColor;

    [Inject] private Timer _timer;
    
    private static string EmissionParameter = "_EmissionColor";
    private static string AlbedoParameter = "_AlbedoColor";
    private Material _tempMaterial;
    private Color _currentColor;

    private Material Material => _meshRenderer.materials[_materialIndex];
    private void Start()
    {
        Material.SetColor(AlbedoParameter, Color.black);
        Material.SetColor(EmissionParameter, _startColor);
        FadeColor(_startColor, _endColor);
    }

    private void FadeColor(Color start, Color end)
    {
        Material.DOColor(end, EmissionParameter, _pulseFadeDelay);
        Material.DOColor(start, EmissionParameter, _pulseFadeDelay)
            .SetDelay(_pulseDelay)
            .OnComplete(() => FadeColor(end, start));

    }
    
    
}
