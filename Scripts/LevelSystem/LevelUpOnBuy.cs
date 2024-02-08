using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class LevelUpOnBuy : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private int _targetLevel;

    [Inject] private LevelSystem _levelSystem;

    private void OnEnable()
    {
        _buyZone.Bought += OnBought;
    }
    
    private void OnDisable()
    {
        _buyZone.Bought -= OnBought;
    }

    private void OnBought()
    {
        _levelSystem.LevelUp(_targetLevel);
    }
}
