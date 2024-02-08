using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UpgradableLinker : Upgradable
{
    protected abstract Upgradable GetUpgradable();
    public override void Upgrade(int upgradeLevel)
    {
        GetUpgradable().Upgrade(upgradeLevel);
    }
}
