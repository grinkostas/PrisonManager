using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK;
using UnityEngine.AI;

public class MovementAnimator : NavMeshMovementAnimatorBase
{
    [SerializeField] private Human _human;

    [SerializeField, ShowIf(nameof(HasAnimator)), AnimatorParam(nameof(BaseAnimator))]
    private string _resetAnimationsParameter;
    protected override Animator Animator => _human.Animator;

    protected override void OnStartMove()
    {
        Animator.SetTrigger(_resetAnimationsParameter);
        base.OnStartMove();
    }
}
