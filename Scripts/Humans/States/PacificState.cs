using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PacificState : State
{
    [SerializeField] private Human _human;
    [SerializeField] private GameObject _pacificView;
    
    [Inject] private MovementPlane _movementPlane;
    public override void OnEnter()
    {
        _pacificView.SetActive(true);
        _human.AgentHandler.Agent.enabled = true;
        _human.AgentHandler.SetDestination(_movementPlane.ReleasedPoint.position);
    }

    public override void OnExit()
    {
        _pacificView.SetActive(false);
    }
}
