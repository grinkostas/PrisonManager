using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Zenject;
using StaserSDK.Utilities;

public class CriminalState : State
{
    [SerializeField] private Criminal _criminal;
    [SerializeField] private float _enableDelay;
    [SerializeField, ShowIf(nameof(_visualize))] private Visualization _visualization;
    [SerializeField] private bool _visualize = true;
    [SerializeField] private Color _outlineColor;
    

    [Inject] private Timer _timer;
    
    public override void OnEnter()
    {
        _timer.ExecuteWithDelay(()=> _criminal.enabled = true, _enableDelay);
        _criminal.enabled = true;
        _criminal.Detect();
        //_criminal.Human.Variant.Outline.enabled = true;
        //_criminal.Human.Variant.Outline.OutlineColor = _outlineColor;
        if(_visualize)
            _visualization.Visualize();
    }

    public override void OnExit()
    {
        _criminal.enabled = false;
        
        
        //_criminal.Human.Variant.Outline.enabled = false;
        if(_visualize)
            _visualization.Dispose();
    }
}
