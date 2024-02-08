using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class CarMovementContorlHandler : MonoBehaviour
{
    [SerializeField] private CarBrakesBase _carBrakes;
    [SerializeField] private CarEngineBase _carEngine;
    [SerializeField] private TrafficLane _trafficLane;
    [SerializeField] private float _minDistanceToNextCar;
    [SerializeField] private float _brakePathLength = 2.2f;
    [SerializeField] private float _positionCorrectionSpeed;
    [SerializeField] private float _startBrakeDistance;
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _offset;

    [Inject] private Player _player;
    public bool IsMoving { get; private set; } = true;

    private Coroutine _brakingCoroutine;

    private List<ICarMovementControl> _affectedCarMovementControl = new List<ICarMovementControl>();

    private void FixedUpdate()
    {
        if (RayCastExtentions.MultipleRaycast(_trafficLane.RayCastPoints, _startBrakeDistance, LayerMask.GetMask("Crosswalk"), out RaycastHit crosswalkHit))
        {
            if (crosswalkHit.collider.TryGetComponent(out ICarMovementControl carMovementControl))
            {
                HandleControl(carMovementControl);
            }
        }
    }

    private void HandleControl(ICarMovementControl carMovementControl)
    {
        if(_affectedCarMovementControl.Contains(carMovementControl))
            return;
        _affectedCarMovementControl.Add(carMovementControl);
        if(_brakingCoroutine != null)
            StopCoroutine(_brakingCoroutine);
        _brakingCoroutine = StartCoroutine(Braking(carMovementControl));
    }
    
    private IEnumerator Braking(ICarMovementControl carMovementControl)
    {
        while(true)
        {
            IsMoving = true;
            float distance = GetDistance(carMovementControl);
            if (carMovementControl.CanMove())
            {
                CorrectPosition(_trafficLane.DistanceToNextCar, _minDistanceToNextCar + _brakePathLength,
                    _minDistanceToNextCar);
                
                
                if (_trafficLane.DistanceToNextCar > _minDistanceToNextCar + _brakePathLength)
                {
                    _carEngine.ForwardGear();
                    _carBrakes.StopBrake(this);
                    if (distance <= _stopDistance - _offset * 2)
                    {
                        break;
                    }
                }

            }
            else
            {
                CorrectPosition(distance, _stopDistance + _brakePathLength, _stopDistance);
            }

            yield return new WaitForFixedUpdate();
        }
        _carEngine.ForwardGear();
        _carBrakes.StopBrake(this);
        _affectedCarMovementControl.Remove(carMovementControl);
        IsMoving = true;
    }

    private void CorrectPosition(float distance, float startBrakeDistance, float stopDistance)
    {
        if (Mathf.Abs(distance - stopDistance) <= _offset)
        {
            _carEngine.ForwardGear();
            _carBrakes.Brake(this);
            IsMoving = false;
            return;
        }
        
        if (distance > stopDistance && distance <= startBrakeDistance)
        {
            _carEngine.ForwardGear();
            _carBrakes.SetTargetSpeed(this, _positionCorrectionSpeed);
            IsMoving = true;
            return;
        }

        if (distance < stopDistance-_offset)
        {
            _carEngine.ReverseGear();
            _carBrakes.SetTargetSpeed(this, _positionCorrectionSpeed);
            IsMoving = true;
            return;
        }
        
        _carEngine.ForwardGear();
        IsMoving = true;

    }

    private float GetDistance(ICarMovementControl carMovementControl)
    {
        Vector2 carVector = new Vector2(transform.position.x, transform.position.z);
        Vector3 controlPosition = carMovementControl.Transform.position;
        Vector2 controlVector = new Vector3(controlPosition.x, controlPosition.z);
        return Vector2.Distance(carVector, controlVector);
    }
}
