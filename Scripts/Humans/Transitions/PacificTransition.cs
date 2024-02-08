using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PacificTransition : StateTransition
{
    [SerializeField] private Scannable _scannable;

    protected override void OnTransitionEnable()
    {
        _scannable.OnScanEnd += OnScanEnd;
    }

    protected override void OnTransitionDisable()
    {
        _scannable.OnScanEnd -= OnScanEnd;
    }

    private void OnScanEnd(Scannable sender, ScanResult result)
    {
        if(result == ScanResult.Pacific)
            Transit();
    }
}
