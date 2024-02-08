using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

public class PlayRunStateTransition : StateTransition
{
    protected override void OnTransitionEnable()
    {
        Transit();
    }
}
