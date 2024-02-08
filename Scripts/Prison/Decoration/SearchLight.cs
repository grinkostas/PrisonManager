using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Zenject;

public class SearchLight : DayPhaseListner
{
    [SerializeField] private float _speed;
    [SerializeField] private DayPhase _targetDayPhase;
    
    [Inject] private MovementPlane _movementPlane;

    private bool _active = false;

    private void Start()
    {
        NextPosition();
    }

    private void NextPosition()
    {
        if(_active == false) return;
        
        Vector3 randomPosition = _movementPlane.GetRandomPosition(transform.position);
        randomPosition.y = transform.position.y;
        var distance = Vector3.Distance(transform.position, randomPosition);
        float time = distance / _speed;
        transform.DOMove(randomPosition, time).OnComplete(NextPosition);
    }

    protected override void OnPhaseChanged(DayPhase phase)
    {
        if (_targetDayPhase == phase)
        {
            _active = true;
            NextPosition();
            return;
        }

        _active = false;
    }
}
