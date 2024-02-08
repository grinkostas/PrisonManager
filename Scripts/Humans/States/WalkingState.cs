using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Zenject;

public class WalkingState : State
{
    [SerializeField] private Human _human;
    [SerializeField] private Scannable _scannable;

    [Inject] private MovementPlane _movementPlane;
    private bool _moving = false;
    private Vector3 _currentDestination;

    
    public override void OnEnter()
    {
        _human.AgentHandler.Agent.enabled = true;
        _moving = true;
        _scannable.gameObject.SetActive(true);
        _scannable.Recovery();
        _human.Variant.DressCasualClothing();
        Move();
        _human.AgentHandler.OnStopMove.AddListener(Move);
    }

    private void Move()
    {
        if(_moving == false) return;
        var destination = _movementPlane.GetDestination(transform);
        _human.AgentHandler.SetDestination(destination);
    }
    
    public override void OnExit()
    {
        _human.AgentHandler.Agent.enabled = false;
        _moving = false;
        
        _human.AgentHandler.OnStopMove.RemoveListener(Move);
        _scannable.gameObject.SetActive(false);
    }
}
