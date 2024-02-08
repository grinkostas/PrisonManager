using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Balance : MonoBehaviour
{
    [SerializeField] private string _balanceId;

    [SerializeField] private float _currentAmount;

    private float CurrentAmount
    {
        get => _currentAmount;
        set
        {
            if (value < 0)
                value = 0;
            _currentAmount = value;
            Save();
            Changed?.Invoke();
        }
    }

    public int Amount => Mathf.CeilToInt(_currentAmount);
    public float RealAmount => _currentAmount;

    public UnityAction Changed;
    public UnityAction<float> Earned;
    
    private void OnEnable()
    {
        _currentAmount = ES3.Load(_balanceId, 20.0f);
    }

    public void Earn(float amount)
    {
        if(amount < 0) return;
        CurrentAmount += amount;
        Earned?.Invoke(amount);
        Changed?.Invoke();
    }

    public bool TrySpend(float amount, bool save = true)
    {
        if (CurrentAmount < amount)
            return false;

        if (save)
        {
            CurrentAmount -= amount;
        }
        else
        {
            _currentAmount -= amount;
            Changed?.Invoke();
        }

        return true;
    }

    public void Save()
    {
        ES3.Save(_balanceId, _currentAmount);
    }
    
    
    
}
