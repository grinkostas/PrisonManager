using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class UpgradesLevelUpTextsInstaller : MonoInstaller
{
    [SerializeField] private UpgradesLevelUpTexts _upgradesLevelUpTexts;

    public override void InstallBindings()
    {
        Container.Bind<UpgradesLevelUpTexts>().FromInstance(_upgradesLevelUpTexts);
    }
}
