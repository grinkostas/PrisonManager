using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Events;

public abstract class Upgradable : MonoBehaviour
{
    public int _currentLevel = 1;

    public UnityAction<Upgradable> Enabled;
    
    private void Start()
    {
        Enabled?.Invoke(this);
    }
    public void UpgradeToLevel(int level)
    {
        _currentLevel = level;
        Upgrade(level); 
    }
    
    public abstract void Upgrade(int upgradeLevel);

}
