using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class CatchTransition : StateTransition
{
    protected override void OnTransitionEnable()
    {
        Criminal.Detected += OnDetected;
    }

    protected override void OnTransitionDisable()
    {
        Criminal.Detected -= OnDetected;
    }

    private void OnDetected(Human Human)
    {
        Transit();
    }
}
