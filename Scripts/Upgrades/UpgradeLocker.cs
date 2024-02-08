using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Upgrades;

public abstract class UpgradeLocker : MonoBehaviour
{
    [SerializeField] private Upgrade _upgrade;
    
    public Upgrade Upgrade => _upgrade;

    public abstract bool IsLocked();
}
