using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarEngine : CarEngineBase
{
    [SerializeField] private Rigidbody _carRigidbody;
    [SerializeField] private float _motorTorque;
    [SerializeField] private Wheels _wheels;

    private float _torqueCoefficient = 1.0f;

    private void FixedUpdate()
    {
        foreach (var wheel in _wheels.All)
            wheel.Collider.motorTorque = _motorTorque * _torqueCoefficient;
    }

    public override void ReverseGear()
    {
        _torqueCoefficient = -1f;
    }

    public override void ForwardGear()
    {
        _torqueCoefficient = 1f;
    }
    
}
