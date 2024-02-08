using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackTakerMoneyGenerator : MoneyGenerator
{
    [SerializeField] private StackTaker _stackTaker;

    private void OnEnable()
    {
        _stackTaker.OnTake += OnTake;
    }

    private void OnDisable()
    {
        _stackTaker.OnTake -= OnTake;
    }

    private void OnTake(StackItem item)
    {
        Generated?.Invoke();
    }
}
