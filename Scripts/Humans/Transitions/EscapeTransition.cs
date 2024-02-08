using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EscapeTransition : StateTransition
{
    [Space]
    [SerializeField] private Prisoner _prisoner;
    [SerializeField] private float _tryEscapeTime;
    [SerializeField, Range(0, 100)] private int _escapeChance;
    [SerializeField, Range(0, 100)] private int _tryChangeReduce;

    private int _tries = 0;
    private float _timer = 0.0f;
    
    private void Update()
    {
        if (TransitionAvailable)
        {
            _timer += Time.deltaTime;
        }
    }

    protected override void OnTransitionEnable()
    {
        _prisoner.Kicked += OnKicked;
    }
    
    protected override void OnTransitionDisable()
    {
        _prisoner.Kicked -= OnKicked;
    }

    private void OnKicked()
    {
        if(_timer < _tryEscapeTime)
            return;
        _timer = 0.0f;
        TryEscape();
    }

    private void TryEscape()
    {
        int random = Random.Range(0, 100);
        _tries++;
        if(random > _escapeChance - (_tries - 1) * _tryChangeReduce)
            return;
        Transit();
    }
    
}
