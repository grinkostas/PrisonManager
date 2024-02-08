using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class StackRadarDisabler : StackCountChangeListner
{
    [Inject] private RadarDisabler _radarDisabler;
    protected override void OnStackCountChanged(int count)
    {
        if(count > 0)
            _radarDisabler.Disable(this);
        else
            _radarDisabler.Enable(this);
    }
}
