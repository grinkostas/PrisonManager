using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class DayPhaseChangerInstaller : MonoInstaller
{
    [SerializeField] private DayPhaseChanger _dayPhaseChanger;

    public override void InstallBindings()
    {
        Container.Bind<DayPhaseChanger>().FromInstance(_dayPhaseChanger);
    }
}
