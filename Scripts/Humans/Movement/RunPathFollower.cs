using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunPathFollower : MonoBehaviour
{
    [SerializeField] private Human _human;
    [SerializeField] private RunPathSelector _pathSelector;

    private Transform _previousDestination = null;
    private RunPath _runPath;
    private int _currentPointIndex = 0;

    public void Run()
    {
        _runPath = _pathSelector.SelectPath();
        StartRun();
    }

    private void StartRun()
    {
        _human.AgentHandler.OnStopMove.AddListener(NextPoint);
        NextPoint();
    }

    private void NextPoint()
    {
        var nextPoint = _runPath.GetTargetPoint(transform, _previousDestination);
        _human.AgentHandler.SetDestination(nextPoint.position);
        _previousDestination = nextPoint;
    }

    public void StopRun()
    {
        _human.AgentHandler.OnStopMove.RemoveListener(NextPoint);
    }
    
}
