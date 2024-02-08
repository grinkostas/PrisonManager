using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ScannableTransition : StateTransition
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

    protected abstract void OnScanEnd(Scannable sender, ScanResult result);
}
