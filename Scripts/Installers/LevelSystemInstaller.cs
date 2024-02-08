using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class LevelSystemInstaller : MonoInstaller
{
    [SerializeField] private LevelSystem _levelSystem;
    public override void InstallBindings()
    {
        Container.Bind<LevelSystem>().FromInstance(_levelSystem);
    }
}
