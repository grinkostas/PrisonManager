using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class BalanceInstaller : MonoInstaller
{
    [SerializeField] private Balance _balance;

    public override void InstallBindings()
    {
        Container.Bind<Balance>().FromInstance(_balance);
    }
}
