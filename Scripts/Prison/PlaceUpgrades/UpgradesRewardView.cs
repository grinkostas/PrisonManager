using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class UpgradesRewardView : MonoBehaviour
{
    [SerializeField] private MoneyReceiver _moneyReceiver;
    [SerializeField] private UpgradesRewardModifier _upgradesRewardModifier;
    [SerializeField] private TMP_Text _baseCostText;
    [SerializeField] private TMP_Text _buffText;

    private void OnEnable()
    {
        Actualize();
        foreach (var model in _upgradesRewardModifier.Models)
        {
            model.Upgraded += Actualize;
        }
    }
    private void OnDisable()
    {
        foreach (var model in _upgradesRewardModifier.Models)
        {
            model.Upgraded -= Actualize;
        }
    }

    private void Actualize()
    {
        _baseCostText.text = _moneyReceiver.BaseReward.ToString();
        float buff = _upgradesRewardModifier.CalculateReward(0f);
        if (buff < 1 || _upgradesRewardModifier == null)
        {
            _buffText.gameObject.SetActive(false);
            return;
        }
        _buffText.gameObject.SetActive(true);
        _buffText.text = $"+{buff}";
    }
    

}
