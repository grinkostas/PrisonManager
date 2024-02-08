using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
  using System.Threading.Tasks;
  using NaughtyAttributes;
  using StaserSDK.Upgrades;
  using Unity.VisualScripting;
  using UnityEngine.Events;
using Zenject;

public class Scannable : MonoBehaviour
{
    [SerializeField] private Human _human;
    [SerializeField] private Collider _collider;
    [SerializeField] private float _distanceToEnableCollider;
    [SerializeField] private bool _upgradableValue = false;
    [SerializeField, HideIf(nameof(_upgradableValue))] private float _scanTime;
    [SerializeField, ShowIf(nameof(_upgradableValue))] private UpgradeValue _scanTimeUpgradeValue;
    [SerializeField] private int _scanOrder;
    [SerializeField] private bool _addOutline = true;
    [SerializeField] private bool _pointing;
    [SerializeField] private float _customUpdateTime = 0.15f;

    [Inject] private Player _player;
    [Inject] private UpgradesController _upgradesController;
    [Inject] private Updater _updater;

    private bool _isScanning = false;
    private bool _scanned = false;
    private float ScanTime => _upgradableValue ? _scanTimeUpgradeValue.GetValue(_upgradesController) : _scanTime;
   
    public int Level => _human.Level;
    public Human Human => _human;
    public int ScanOrder => _scanOrder;

    public Collider Collider => _collider;
    
    public UnityAction OnEnterScanZone;
    public UnityAction<float> ScanProgressChanged;
    public UnityAction<Scannable, ScanResult>  OnScanEnd;
    public UnityAction OnExitScanZone;

    public bool Scanned => _scanned;
    
    

    private void OnEnable()
    {
        _scanned = false;
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(_scanned) return;
        
        if (other.TryGetComponent(out Radar radar))
        {
            OnEnterScanZone?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_isScanning || _scanned) return;
        
        if (other.TryGetComponent(out Radar radar))
        {
            var status = radar.GetStatus(this);
            
            if(status == ScanStatus.Scan) StartScan(radar);
        }
    }

    private void StartScan(Radar radar)
    {
        radar.StartScan(this);
        _isScanning = true;
        StartCoroutine(Scanning(radar));
    }

    private void StopScan(Radar radar)
    {
        radar.StopScan(this);
        _isScanning = false;
    }

    private IEnumerator Scanning(Radar radar)
    {
        float progress = 0;
        float wastedTime = 0;
        
        if(_addOutline)
            _human.Variant.Outline.enabled = true;
        
        while (_isScanning && wastedTime <= ScanTime)
        {
            if (radar.Target != this || radar.enabled == false || radar.gameObject.activeSelf == false)
            {
                BreakScan();
                yield break;
            }

            wastedTime += Time.deltaTime;
            progress = wastedTime / ScanTime;
            ScanProgressChanged?.Invoke(progress);
            yield return null;
        }
        if (wastedTime >= ScanTime)
            SuccessfulScan(radar);
    }

    private void SuccessfulScan(Radar radar)
    {
        _scanned = true;
        if(_addOutline)
            _human.Variant.Outline.enabled = false;
        var scanResult = radar.GetScanResult(this);
        OnScanEnd?.Invoke(this, scanResult);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(_scanned) return;
        if (other.TryGetComponent(out Radar radar))
        {
            OnExitScanZone?.Invoke();
            BreakScan();
            StopScan(radar);
        }
    }

    private void BreakScan()
    {
        if(_addOutline)
            _human.Variant.Outline.enabled = false;
        _isScanning = false;
    }

    public void Recovery()
    {
        _isScanning = false;
        _scanned = false;
    }

}
