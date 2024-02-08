using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PrisonInstaller : MonoInstaller
{
    [SerializeField] private Prison _prison;

    public override void InstallBindings()
    {
        Container.Bind<Prison>().FromInstance(_prison);
    }
}
