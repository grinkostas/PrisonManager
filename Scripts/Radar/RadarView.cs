using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using Zenject;

public class RadarView : MonoBehaviour
{
    [SerializeField] private AlphaView _radarView;
    [SerializeField] private float _additionCheckTime = 0.25f;
    private List<Scannable> _scannablesInZone = new List<Scannable>();

    [Inject] private Updater _updater;

    private void OnEnable()
    {
        _updater.Add(this, OnUpdate, _additionCheckTime);
    }

    private void OnDisable()
    {
        _scannablesInZone.Clear();
        _radarView.ForceHide();
        _updater.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Scannable scannable))
        {
            Add(scannable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Scannable scannable))
        {
            Remove(scannable);
        }
    }

    private void OnUpdate()
    {
        if (_scannablesInZone.Count == 0)
        {
            return;
        }
        var scannablesInZoneTemp = new List<Scannable>(_scannablesInZone);
        foreach (var scannable in scannablesInZoneTemp)
        {
            if(scannable.enabled == false)
                Remove(scannable);
        }
    }
    

    private void Add(Scannable scannable)
    {
        if(_scannablesInZone.Contains(scannable) || scannable.Scanned) 
            return;

        scannable.OnScanEnd += OnScanned; 
        _scannablesInZone.Add(scannable);
        
        _radarView.Show();
    }

    private void Remove(Scannable scannable)
    {
        scannable.OnScanEnd -= OnScanned; 
        _scannablesInZone.Remove(scannable);
        if(_scannablesInZone.Count > 0) return;
        _radarView.Hide();
    }
    

    private void OnScanned(Scannable sender, ScanResult result)
    {
        List<Scannable> scannablesToRemove = new List<Scannable>();
        foreach (var scannable in _scannablesInZone)
        {
            if(scannable.Scanned || scannable.enabled == false)
                scannablesToRemove.Add(scannable);
        }
        
        scannablesToRemove.ForEach(x=> Remove(x));
    }
    
}
