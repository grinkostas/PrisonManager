using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorExtention : ActionPlaceExtention
{
    [SerializeField] private bool _rootMotion;


    protected override void OnUsePlace(Prisoner prisoner)
    {
        prisoner.Human.Animator.applyRootMotion = _rootMotion;
    }

    protected override void OnKick(Prisoner prisoner)
    {
        prisoner.Human.Animator.applyRootMotion = !_rootMotion;
    }
}
