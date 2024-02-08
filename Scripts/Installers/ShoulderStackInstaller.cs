using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class ShoulderStackInstaller : MonoInstaller
{
    [SerializeField] private Stack _stack;

    public override void InstallBindings()
    {
        Container.Bind<Stack>().FromInstance(_stack);
    }
}
