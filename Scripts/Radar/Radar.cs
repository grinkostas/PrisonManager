using System.Collections.Generic;
using NepixCore.Game.API;
using StaserSDK.Upgrades;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[RequireComponent(typeof(Collider))]
public class Radar : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int _criminalChance;
    [SerializeField, Range(0, 100)] private int _unsuccessfulModifier;
    [SerializeField] private Collider _collider;
    [SerializeField] private int _maxCriminals;

    [Inject] private LevelSystem _levelSystem;
    [Inject] private IHapticService _hapticService;
    
    private int _unsuccessfulCount = 0;
    private Scannable _currentScanTarget = null;

    private bool _overrideChance = false;
    private int _nextScanChange;
    
    public UnityAction<Human> DetectedCriminal;
    public UnityAction<Human> ScanStarted;
    public Scannable Target => _currentScanTarget;

    
    
    public void ChangeNextScanChange(int change)
    {
        _overrideChance = true;
        _nextScanChange = change;
    }

    private int GetScanChance()
    {
        return _overrideChance ? _nextScanChange :_criminalChance;
    }
    
    public ScanStatus GetStatus(Scannable scannable)
    {
        if (scannable.Level > _levelSystem.CurrentLevel)
            return ScanStatus.Unavailable;
        
        if (_currentScanTarget != null && _currentScanTarget.Scanned)
            _currentScanTarget = null;
        
        if (_currentScanTarget == null)
            return ScanStatus.Scan;
        
        if (_currentScanTarget == scannable)
            return ScanStatus.Scan;

        if (scannable.ScanOrder > _currentScanTarget.ScanOrder)
            return ScanStatus.Scan;
        
        return ScanStatus.Wait;
    }

    public void StartScan(Scannable scannable)
    {
        ScanStarted?.Invoke(scannable.Human);
        _currentScanTarget = scannable;
    }

    public void StopScan(Scannable scannable)
    {
        if (_currentScanTarget == scannable)
            _currentScanTarget = null;
    }

    public ScanResult GetScanResult(Scannable scannable)
    {
        int currentChance = Mathf.Clamp(GetScanChance() + _unsuccessfulModifier * _unsuccessfulCount, 0, 100);
        int random = Random.Range(0, 100);
        if (currentChance >= random)
        {
            _unsuccessfulCount = 0;
            DetectedCriminal?.Invoke(scannable.Human);
            _overrideChance = false;
            _hapticService.Selection();
            return ScanResult.Criminal;
        }  
        
        _unsuccessfulCount++;
        
        _hapticService.Selection();
        
        if (_currentScanTarget == scannable)
            _currentScanTarget = null;
        
        return ScanResult.Pacific;
    }


    public void Disable()
    {
        _collider.enabled = false;
    }

    public void Enable()
    {
        _collider.enabled = true;
    }
    
}
