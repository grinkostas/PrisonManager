using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpgradesRewardModifier : MoneyRewardModifier
{
    [SerializeField] private List<PlaceUpgradeModel> _placeUpgradeModels;

    public List<PlaceUpgradeModel> Models => _placeUpgradeModels;

    public override float CalculateReward(float reward)
    {
        return reward + _placeUpgradeModels.Sum(x => x.Value);
    }
}
