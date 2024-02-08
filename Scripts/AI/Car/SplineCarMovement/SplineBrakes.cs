using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;

public class SplineBrakes : CarBrakesBase
{
    [SerializeField] private SplineFollower _splineFollower;
    [SerializeField] private SplineCarEngine _carEngine;
    [SerializeField] private float _brakeSpeed;
    [SerializeField] private float _accelerationSpeed;

    protected override float Speed => _splineFollower.followSpeed;

    protected override void ActualizeSpeed()
    {
        float speed = _carEngine.GearMultiplier * TargetSpeed;
        if (Brakes.Count == 0)
            speed = _carEngine.DefaultSpeed;
        
        if(Mathf.Abs(Speed - speed) < 0.05f)
            return;
        
        float speedToSet = Speed;
        
        if (Speed > speed)
        {
            speedToSet -= Time.fixedDeltaTime * _brakeSpeed;
        }
        else
        {
            speedToSet += Time.fixedDeltaTime * _accelerationSpeed;
        }
        _splineFollower.followSpeed = speedToSet;
    }
}
