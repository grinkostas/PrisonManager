using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunTransition : ScannableTransition
{
    protected override void OnScanEnd(Scannable sender, ScanResult result)
    {
        if(result == ScanResult.Criminal)
            Transit();
    }
}
