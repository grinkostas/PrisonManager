using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Zenject;

public class ReleasedState : State
{
    [SerializeField] private Human _human;
    [SerializeField] private Animator _baseHumanAnimator;
    
    [SerializeField, AnimatorParam(nameof(_baseHumanAnimator))]
    private string _releasedTrigger;

    [Inject] private MovementPlane _movementPlane;

    private bool _entered = false;
    
    public override void OnEnter()
    {
        _entered = true;
        Debug.Log($"Released to {_movementPlane.ReleasedPoint.position}");
        _human.AgentHandler.SetDestination(_movementPlane.ReleasedPoint.position);
        _human.Animator.SetTrigger(_releasedTrigger);
    }

    public override void OnExit()
    {
        _entered = false;
    }
}
