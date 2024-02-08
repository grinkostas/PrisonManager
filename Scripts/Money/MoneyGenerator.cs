using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MoneyGenerator : MonoBehaviour
{
    [SerializeField] private List<MoneyRewardModifier> _rewardModifiers;
    
    public UnityAction Generated;

    public float CalculateReward(float startReward)
    {
        foreach (var rewardModifier in _rewardModifiers)
        {
            startReward = rewardModifier.CalculateReward(startReward);
        }

        return startReward;
    }
}
