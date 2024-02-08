using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScanProgress : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Scannable _scannable;

    private void OnEnable()
    {
        _model.SetActive(false);
        _scannable.ScanProgressChanged += OnScanProgressChanged;
        _scannable.OnExitScanZone += OnExitScanZone;
        _scannable.OnScanEnd += OnScanEnd;
    }
    
    private void OnDisable()
    {
        _scannable.ScanProgressChanged -= OnScanProgressChanged;
        _scannable.OnExitScanZone -= OnExitScanZone;
        _scannable.OnScanEnd -= OnScanEnd;
    }

    private void OnScanProgressChanged(float progress)
    {
        _progressSlider.value = 1 - progress;
        if(progress > 0.1f)
            _model.SetActive(true);
    }

    private void OnExitScanZone()
    {
        _model.SetActive(false);
        _progressSlider.value = 1;
    }

    private void OnScanEnd(Scannable sender, ScanResult result)
    {
        _model.SetActive(false);
    }
}
