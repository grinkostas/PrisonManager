using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class ScanLevelInfo : MonoBehaviour
{
    [SerializeField] private Scannable _scannable;
    [SerializeField] private View _levelView;

    [Inject] private LevelSystem _levelSystem;
    [Inject] private RadarDisabler _radarDisabler;
    private Radar Radar => _radarDisabler.Radar;
    private void OnEnable()
    {
        _scannable.OnEnterScanZone += OnEnterScanZone;
        Radar.ScanStarted += OnScanStarted;
    }
    
    private void OnDisable()
    {
        _scannable.OnEnterScanZone -= OnEnterScanZone;
        Radar.ScanStarted -= OnScanStarted;
    }

    private void OnEnterScanZone()
    {
        if (_scannable.Human.Level > _levelSystem.CurrentLevel)
        {
            _levelView.Show();
        }
    }

    private void OnScanStarted(Human human)
    {
        _levelView.Hide();
    }
}
