using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyTransition : StateTransition
{
    [SerializeField] private BuyZone _buyZone;
    
    protected override void OnTransitionEnable()
    {
        _buyZone.gameObject.SetActive(true);
        _buyZone.Bought += OnBuy;
    }

    protected override void OnTransitionDisable()
    {
        _buyZone.Bought -= OnBuy;
    }

    private void OnBuy()
    {
        Transit();
    }
}
