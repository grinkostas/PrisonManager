using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class RadarDisablerInstaller : MonoInstaller
{
    [SerializeField] private RadarDisabler _radarDisabler;

    public override void InstallBindings()
    {
        Container.Bind<RadarDisabler>().FromInstance(_radarDisabler);
    }
}
