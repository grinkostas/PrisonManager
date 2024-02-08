using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MoneyRewardModifier : MonoBehaviour
{
    public abstract float CalculateReward(float reward);
}
