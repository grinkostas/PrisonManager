using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Upgrades;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

[System.Serializable]
public class PlaceUpgradeModel : MonoBehaviour
{
    [SerializeField] private int _startLevel;
    [SerializeField] private int _maxLevel;
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private UpgradeProperty _priceProperty;
    [SerializeField] private UpgradeProperty _buffProperty;
    [SerializeField] private string _saveId;
    

    [Inject] private Balance _balance;

    public float Value => _buffProperty.Calculate(GetLevel());
    public float NextValue => _buffProperty.Calculate(GetLevel() + 1);
    public int Price => (int)_priceProperty.Calculate(GetLevel());
    public bool CanLevelUp => GetLevel() <= _maxLevel;

    public UnityAction Upgraded;
    
    private void Start()
    { 
        _upgrader.Upgrade(GetLevel());
    }

    public void Upgrade()
    {
        if(CanLevelUp == false)
            return;
        
        if (_balance.TrySpend(Price))
        {
            ES3.Save(_saveId, GetLevel() + 1);
            Upgraded?.Invoke();
            _upgrader.Upgrade(GetLevel());
        }
    }

    public int GetLevel()
    {
        return ES3.Load(_saveId, _startLevel);
    }
}
