using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class HandStackInstaller : MonoInstaller
{
    [SerializeField] private HandStack _handStack;

    public override void InstallBindings()
    {
        Container.Bind<HandStack>().FromInstance(_handStack);
    }
}
