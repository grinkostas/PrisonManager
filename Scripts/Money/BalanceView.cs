using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Zenject;

public class BalanceView : MonoBehaviour
{
    [SerializeField] private TMP_Text _balanceText;
    
    [Inject] private Balance _balance;

    private void OnEnable()
    {
        _balance.Changed += Actualize;
    }

    private void OnDisable()
    {
        _balance.Changed -= Actualize;
    }

    private void Start()
    {
        Actualize();
    }

    private void Actualize()
    {
        _balanceText.text = _balance.Amount.ToString();
    }
}
