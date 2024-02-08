using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Upgrades;
using Zenject;

public class UpgradesShop : UiView
{
    [SerializeField] private UpgradeShopView _shopViewPrefab;
    [SerializeField] private Transform _viewsParent;
    [SerializeField] private List<UpgradeShopItem> _shopItems;
    [SerializeField] private List<UpgradeLocker> _upgradeLockers = new List<UpgradeLocker>();

    [Inject] private DiContainer _diContainer;
    
    private List<UpgradeShopView> _spawnedViews = new List<UpgradeShopView>();

    private void Start()
    {
        CreateViews();
    }

    private void CreateViews()
    {
        if(_spawnedViews.Count > 0)
            return;
        
        foreach (var shopItem in _shopItems)
        {
            var viewObject = _diContainer.InstantiatePrefab(_shopViewPrefab, _viewsParent);
            var shopItemView = viewObject.GetComponent<UpgradeShopView>();
            _spawnedViews.Add(shopItemView);
            shopItemView.Init(shopItem);
        }

        ActualizeViews();
    }

    private void ActualizeViews()
    {
        foreach (var upgradeShopView in _spawnedViews)
        {
            if (IsLocked(upgradeShopView.ShopItem))
                upgradeShopView.Lock();
            else
                upgradeShopView.UnLock();
        }
    }

    private bool IsLocked(UpgradeShopItem shopItem)
    {
        UpgradeLocker locker = _upgradeLockers.Find(x => x.Upgrade.Id == shopItem.Upgrade.Id);
        if (locker == null)
            return false;
        
        return locker.IsLocked();
    }

    [Button("Show")]
    public override void Show()
    {
        ActualizeViews();
        base.Show();
    }
}
