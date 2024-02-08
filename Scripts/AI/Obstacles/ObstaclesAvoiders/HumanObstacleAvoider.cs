using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;

public class HumanObstacleAvoider : ObstaclesAvoider
{
    [SerializeField] private List<Transform> _raycastPoints;
    [SerializeField] private NavMeshAgentHandler _handler;

    private float _startSpeed;
    private void Awake()
    {
        _startSpeed = _handler.Agent.speed;
    }

    public override List<Transform> GetRaycastPoints() => _raycastPoints;

    protected override void StartAvoid()
    {
        _startSpeed = _handler.Agent.speed;
        _handler.Agent.speed = 0.0f;
    }

    protected override void StopAvoid()
    {
        _handler.Agent.speed = _startSpeed;
    }
}
