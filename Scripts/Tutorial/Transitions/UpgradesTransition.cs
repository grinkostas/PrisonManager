using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StaserSDK.Upgrades;
using Zenject;

public class UpgradesTransition : StateTransition
{
    [SerializeField] private Upgrade _upgrade;
    
    [Inject] private UpgradesController _upgradesController;

    private UpgradeModel Model => _upgradesController.GetModel(_upgrade);

    protected override void OnTransitionEnable()
    {
        Model.Upgraded += OnUpgraded;
    }

    protected override void OnTransitionDisable()
    {
        Model.Upgraded -= OnUpgraded;
    }

    private void OnUpgraded()
    {
        Transit();
    }
}
