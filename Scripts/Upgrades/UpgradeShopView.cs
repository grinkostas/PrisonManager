using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Upgrades;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class UpgradeShopView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _buyButton;
    
    [SerializeField] private GameObject _availableToBuyView;
    [SerializeField] private GameObject _lockedView;
    [SerializeField] private GameObject _maxView;

    [Inject] private UpgradesController _upgradeController;
    [Inject] private Balance _balance;

    private List<GameObject> Views => new() { _availableToBuyView, _lockedView, _maxView };
    private float Price => _shopItem.Price.Calculate(_upgradeModel.CurrentLevel);

    private UpgradeShopItem _shopItem;
    private UpgradeModel _upgradeModel;

    private bool _isLocked = false;

    public UpgradeShopItem ShopItem => _shopItem;

    private void OnEnable()
    {
        Actualize();
    }

    public void Init(UpgradeShopItem shopItem)
    {
        _shopItem = shopItem;
        _upgradeModel = _upgradeController.GetModel(_shopItem.Upgrade);
        Actualize();
    }

    private void Actualize()
    {
        if(_shopItem == null) 
            return;

        _iconImage.sprite = _upgradeModel.Upgrade.Icon;
        _title.text = _shopItem.Upgrade.Name;
        _priceText.text = ((int)Price).ToString();
        float sliderValue = (float)_upgradeModel.CurrentLevel / (float)_upgradeModel.MaxLevel;
        _progressSlider.value = sliderValue;
        
        _buyButton.interactable = !(_balance.Amount < (int)Price);
        
        if(_isLocked == false)
            SwitchView(_availableToBuyView);
        else
            SwitchView(_lockedView);

        if (_upgradeModel.CanLevelUp() == false)
            SwitchView(_maxView);

    }

    private void SwitchView(GameObject targetView)
    {
        foreach (var view in Views)
            view.SetActive(false);
        targetView.SetActive(true);
    }


    public void Lock()
    {
        _isLocked = true; 
        Actualize();
    }

    public void UnLock()
    {
        _isLocked = false; 
        Actualize();
    } 

    public void Buy()
    {
        if(_upgradeModel.CanLevelUp() == false || _balance.Amount < (int)Price)
            return;
        _balance.TrySpend(Price);
        _upgradeModel.LevelUp();
        Actualize();
    }

}
