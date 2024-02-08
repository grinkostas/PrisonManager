using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CarBrakes : CarBrakesBase
{
    [SerializeField] private Rigidbody _carRigidbody;
    [SerializeField] private Wheels _wheels;
    [SerializeField] private float _brakeForce;
    
    private List<SpeedData> _brakes = new List<SpeedData>();

    protected override float Speed => _carRigidbody.velocity.magnitude;

    protected override void ActualizeSpeed()
    {
        if (_brakes.Count == 0)
        {
            Brake(0);
            return;
        }
        if (Speed > TargetSpeed)
            Brake();
        else
            Brake(0f);
    }

    public override void Brake(object sender)
    {
        TargetSpeed = 0.0f;
        if (_brakes.Contains(sender))
        {
            _brakes.Find(x => x.Sender == sender).TargetSpeed = TargetSpeed;
            return;
        }
        _brakes.Add(new SpeedData(sender));
    }

    public override void StopBrake(object sender)
    {
        _brakes.RemoveAll(x => x.Sender == sender);
        TargetSpeed = GetMinSpeed();
    }

    public override void SetTargetSpeed(object sender, float targetSpeed)
    {
        if (targetSpeed < TargetSpeed)
            TargetSpeed = targetSpeed;
        if (_brakes.Contains(sender))
        {
            _brakes.Find(x => x.Sender == sender).TargetSpeed = targetSpeed;
            return;
        }
        _brakes.Add(new SpeedData(sender, targetSpeed));
    }

    private float GetMinSpeed()
    {
        if(_brakes.Count > 0)
            return _brakes.Min(x => x.TargetSpeed);
        return float.PositiveInfinity;
    }


    private void Brake() => Brake(_brakeForce);
    
    private void Brake(float force)
    {
        if (TargetSpeed < 0.001f && Speed < 0.05f)
        {
            _carRigidbody.velocity = Vector3.zero;
        }
        foreach (var wheel in _wheels.All)
        {
            wheel.Collider.brakeTorque = force;
        }
    }
    
    
}
