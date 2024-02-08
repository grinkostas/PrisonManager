using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class EscapeState : State
{
    [SerializeField] private Human _human;
    [SerializeField] private float _escapeSpeed;
    [Inject] private MovementPlane _movementPlane;

    private void Update()
    {
        if (enabled)
        {
            _human.AgentHandler.Agent.speed = _escapeSpeed;
        }
    }

    public override void OnEnter()
    {
        _human.Prisoner.Escape();
        _human.AgentHandler.Agent.enabled = true;
        _human.AgentHandler.SetDestination(_movementPlane.ReleasedPoint.position);
        _human.AgentHandler.Agent.speed = _escapeSpeed;
    }

    public override void OnExit()
    {
    }
}
