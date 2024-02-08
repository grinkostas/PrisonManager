using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceUpgrader : Upgrader
{
    [SerializeField] private List<BuyZone> _upgradeZones;
    private void OnEnable()
    {
        foreach (var buyZone in _upgradeZones)
        {
            if (buyZone.IsBought)
            {
                Upgrade();
                continue;
            }

            buyZone.Bought += Upgrade;
        }
    }

    private void OnDisable()
    {
        foreach (var buyZone in _upgradeZones)
        {
            buyZone.Bought -= Upgrade;
        }
        
    }
}
