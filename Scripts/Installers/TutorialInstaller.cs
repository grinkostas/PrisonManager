using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    [SerializeField] private AboveArrow _aboveArrowPrefab;
    [SerializeField] private HumanSpawner _humanSpawner;
    public override void InstallBindings()
    {
        Container.Bind<HumanSpawner>().FromInstance(_humanSpawner);
        Container.Bind<AboveArrow>().FromInstance(_aboveArrowPrefab);
    }
}
