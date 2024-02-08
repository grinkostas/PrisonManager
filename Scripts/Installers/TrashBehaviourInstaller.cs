using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class TrashBehaviourInstaller : MonoInstaller
{
    [SerializeField] private TrashBehaviour _trashBehaviour;

    public override void InstallBindings()
    {
        Container.Bind<TrashBehaviour>().FromInstance(_trashBehaviour);
    }
}
