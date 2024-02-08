using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Upgrades;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class PlaceUpgradeView : MonoBehaviour
{
    [SerializeField] private PlaceUpgradeModel _placeUpgradeModel;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _buffText;

    [Inject] private Balance _balance;
    
    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
        _balance.Earned += OnEarned;
        Actualize();
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
        _balance.Earned -= OnEarned;
    }
    
    private void Actualize()
    {
        _priceText.text = _placeUpgradeModel.Price.ToString();
        _buffText.text = $"+{_placeUpgradeModel.NextValue-_placeUpgradeModel.Value}";

        _upgradeButton.interactable = _balance.Amount >= _placeUpgradeModel.Price && _placeUpgradeModel.CanLevelUp;

    }

    private void OnEarned(float value) => Actualize();
    private void OnUpgradeButtonClick()
    {
        _placeUpgradeModel.Upgrade();
        Actualize();
    }
    
}
