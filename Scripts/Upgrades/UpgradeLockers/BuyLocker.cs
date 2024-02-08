using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyLocker : UpgradeLocker
{
    [SerializeField] private BuyZone _buyZone;
    
    public override bool IsLocked()
    {
        return !_buyZone.IsBought;
    }
}
