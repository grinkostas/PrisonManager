using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookAtExtention : ActionPlaceExtention
{
    [SerializeField] private Transform _lookAtTarget;
    protected override void OnUsePlace(Prisoner prisoner)
    {
        prisoner.transform.LookAt(_lookAtTarget);
    }

    protected override void OnKick(Prisoner prisoner)
    {
    }
}
