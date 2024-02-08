using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class MovementPlaneInstaller : MonoInstaller
{
    [SerializeField] private MovementPlane _movementPlane;

    public override void InstallBindings()
    {
        Container.Bind<MovementPlane>().FromInstance(_movementPlane);
    }
}
