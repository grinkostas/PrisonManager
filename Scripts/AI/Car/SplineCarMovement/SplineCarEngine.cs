using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineCarEngine : CarEngineBase
{
    [SerializeField] private bool _reverse;
    [SerializeField] private CarBrakesBase _carBrakes;
    [SerializeField] private float _defaultSpeed;

    public float DefaultSpeed => _defaultSpeed;
    public float GearMultiplier { get; private set; } = 1;

    private float _reverseMultiplier => _reverse ? -1 : 1;
    
    public override void ReverseGear()
    {
        GearMultiplier = -1 * _reverseMultiplier;
    }

    public override void ForwardGear()
    {
        GearMultiplier = 1 * _reverseMultiplier;
    }
}
