using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CriminalTransition : ScannableTransition
{
    protected override void OnScanEnd(Scannable sender, ScanResult result)
    {
        Transit();
    }
}
