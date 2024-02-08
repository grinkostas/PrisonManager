using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class AnimatorInstaller : MonoInstaller
{
    [SerializeField] private Animator _animator;

    public override void InstallBindings()
    {
        Container.Bind<Animator>().FromInstance(_animator);
    }
    
}
