using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeMissionTransition : MissionTransition
{
    [SerializeField] private List<PlaceUpgradeModel> _placeUpgradeModel;
    
    
    protected override void OnMissionEnter()
    {
        Debug.Log($"upgarde mission enter");
        foreach (var placeUpgradeModel in _placeUpgradeModel)
        {
            placeUpgradeModel.Upgraded += Complete;
        }
        
    }

    protected override float GetProgress()
    {
        return CurrentState.Completed ? 1 : 0;
    }

    protected override void OnTransitionDisable()
    {
        foreach (var placeUpgradeModel in _placeUpgradeModel)
        {
            placeUpgradeModel.Upgraded -= Complete;
        }
    }

    protected override bool AdditionSkipCondition()
    {
        foreach (var placeUpgradeModel in _placeUpgradeModel)
        {
            if (placeUpgradeModel.CanLevelUp)
                return false;
        }

        return true;
    }
}
