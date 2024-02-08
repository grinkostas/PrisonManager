using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class ScheduleInstaller : MonoInstaller
{
    [SerializeField] private Schedule _schedule;
    public override void InstallBindings()
    {
        Container.Bind<Schedule>().FromInstance(_schedule);
    }
}
