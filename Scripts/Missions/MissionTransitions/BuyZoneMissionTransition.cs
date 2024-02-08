using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyZoneMissionTransition : MissionTransition
{
    [SerializeField] private BuyZone _buyZone;
    
    protected override void OnMissionEnter()
    {
        _buyZone.Bought += Complete;
        _buyZone.BuyProgressChanged += OnProgressChanged;;
    }

    private void OnProgressChanged(float progress)
    {
        CurrentState.ProgressChanged?.Invoke(progress/_buyZone.Price);
        if(Mathf.Abs(1-progress) < 0.01f)
            Complete();
    }

    protected override void OnTransitionDisable()
    {
        _buyZone.Bought -= Complete;
        _buyZone.BuyProgressChanged -= OnProgressChanged;
    }

    protected override float GetProgress()
    {
        return _buyZone.Spend / _buyZone.Price;
    }

    protected override bool AdditionSkipCondition()
    {
        return _buyZone.IsBought || _buyZone.gameObject.activeInHierarchy == false;
    }
}
