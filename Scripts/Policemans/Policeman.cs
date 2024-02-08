using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Policeman : MonoBehaviour
{
    [SerializeField] private NavMeshAgentHandler _agentHandler;
    [SerializeField] private List<Transform> _movePoints;

    private int _currentPointIndex = -1;

    private void OnEnable()
    {
        _agentHandler.OnStopMove.AddListener(NextPoint);
    }

    private void Start()
    {
        NextPoint();
    }

    private void NextPoint()
    {
        var nextPoint = GetNextPoint();
        _agentHandler.SetDestination(nextPoint.position);
    }

    private Transform GetNextPoint()
    {
        _currentPointIndex++;
        if (_currentPointIndex >= _movePoints.Count)
            _currentPointIndex = 0;
        return _movePoints[_currentPointIndex];
    }
    
    
}
