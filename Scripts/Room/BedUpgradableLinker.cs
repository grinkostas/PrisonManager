using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BedUpgradableLinker : UpgradableLinker
{
    [SerializeField] private BedModel _bedModel;
    
    protected override Upgradable GetUpgradable()
    {
        return _bedModel.Variant;
    }
}
