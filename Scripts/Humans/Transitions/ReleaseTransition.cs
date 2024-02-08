using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReleaseTransition : StateTransition
{
    [SerializeField] private Prisoner _prisoner;

    protected override void OnTransitionEnable()
    {
        _prisoner.OnRelease += Transit;
    }

    protected override void OnTransitionDisable()
    {
        _prisoner.OnRelease -= Transit;
    }
}
