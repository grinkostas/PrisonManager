using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarObstacleAvoider : ObstaclesAvoider
{
    [SerializeField] private TrafficLane _trafficLane;
    [SerializeField] private CarBrakesBase _carBrakes;

    public override List<Transform> GetRaycastPoints() => _trafficLane.RayCastPoints;

    protected override void StartAvoid()
    {
        _carBrakes.Brake(this);
    }

    protected override void StopAvoid()
    {
        _carBrakes.StopBrake(this);
    }
}
