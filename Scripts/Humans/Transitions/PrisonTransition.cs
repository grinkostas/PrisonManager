using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrisonTransition : StateTransition
{
    private void OnTriggerEnter(Collider other)
    {
        if(TransitionAvailable == false) return;
        if (other.TryGetComponent(out PrisonEntrance entrance))
        {
            Transit();
        }
    }
}
