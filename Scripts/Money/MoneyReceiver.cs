using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MoneyReceiver : MonoBehaviour
{
    [SerializeField] private MoneyStack _moneyStack;
    [SerializeField] private MoneyGenerator _moneyGenerator;
    [SerializeField] private float _baseCost;

    public float BaseReward => _baseCost;
    
    private void OnEnable()
    {
        _moneyGenerator.Generated += Receive;
    }

    private void OnDisable()
    {
        _moneyGenerator.Generated -= Receive;
    }

    protected void Receive()
    {
        var cost = _moneyGenerator.CalculateReward(_baseCost);
        _moneyStack.Add(cost, transform);
    }

}
