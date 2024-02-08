using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private CarBrakesBase _carBrakes;
    [SerializeField] private Wheels _wheels;
    [SerializeField] private Transform _carFrontPoint;
    [SerializeField] private float _targetSpeed;
    [SerializeField] private float _targetRotateSpeed;
    [SerializeField] private float _startRotateOffset;
    [SerializeField] private float _startBrakeBeforeRotateOffset;
    [SerializeField] private float _updateDelay = 0.1f;
    [Space] 
    [SerializeField] private CarMovementContorlHandler _carMovementContorlHandler;
    private Vector3 _currentDestination = Vector3.negativeInfinity;
    private Transform _currentDestinationTransform;
    
    private bool _rotating = false;
    private bool _receivedDestination = false;
    private Side _startRotateSide;

    private float _torqueCoefficient = 1.0f;

    public UnityAction OnReceiveDestination;


    private void Start()
    {
        _currentTargetSpeed = _targetSpeed;
        _carBrakes.SetTargetSpeed(this, _targetSpeed);
        StartCoroutine(Rotating());
    }

    private void Update()
    {
        if (Distance() < 1.0f)
        {
            OnReceiveDestination?.Invoke();
        }
    }

    private float Distance()
    {
        return Mathf.Pow(_currentDestination.x - _carFrontPoint.position.x, 2)
               + Mathf.Pow(_currentDestination.y - _carFrontPoint.position.y, 2)
               + Mathf.Pow(_currentDestination.z - _carFrontPoint.position.z, 2);
    }


    private IEnumerator Rotating()
    {
        while (true)
        {
            if (_currentDestination == Vector3.negativeInfinity)
            {
                yield return new WaitForSeconds(_updateDelay);
                continue;
            }

            Rotate();
            yield return new WaitForSeconds(_updateDelay);
        }
    }
    
    
    private void Rotate()
    {
        if(_carMovementContorlHandler.IsMoving == false)
            return;
        
        PreRotationBrake();
        
        if(HaveToRotate() == false)
            return;

        StartRotate();
        
        if (HaveToStopRotate())
            return;
        
        RotateWheels();
    }

    private void StartRotate()
    {
        if (_rotating == false)
        {
            _startRotateSide = GetSide();
            _rotating = true;
        }
    }

    private bool HaveToStopRotate()
    {
        if (GetDot() >= 0.98f && GetSideDistance(GetSide().Invert()) <= 0.01f)
        {
            StopRotate();
            return true;
        }

        return false;
    }
    private bool HaveToRotate()
    {
        if( GetForwardDistance() > _startRotateOffset && GetDot() < 0.6f)
            return false;
        return true;
    }

    private float _currentTargetSpeed = float.PositiveInfinity;
    private void RotateWheels()
    {
        if (Math.Abs(_currentTargetSpeed - _targetRotateSpeed * 2) > 0.01f)
        {
            _currentTargetSpeed = _targetSpeed * 2;
            _carBrakes.SetTargetSpeed(this, _currentTargetSpeed);
        }
        float rotateAngle = GetAngle();

        float coefficient = Mathf.Clamp(rotateAngle / _wheels.MaxSteeringAngle, 0, 1f);

        _wheels.Rotate(coefficient * GetSideAdditionalRotateCoefficient());
    }
    
    private float GetSideAdditionalRotateCoefficient()
    {
        float sideCoefficient = GetSideCoefficient(_startRotateSide);
        Side side = GetSide(_carFrontPoint.forward, 0.75f);
            
        if (side != Side.Forward)
            sideCoefficient = GetSideCoefficient(side);
        
        return sideCoefficient;
    }

    private float GetAngle()
    {
        return  Mathf.Clamp(Vector2.Angle(
                new Vector2(_currentDestinationTransform.forward.x, _currentDestinationTransform.forward.z),
                new Vector2(_carFrontPoint.forward.x, _carFrontPoint.forward.z)
            ), 
            0, 90f);
    }

    private void PreRotationBrake()
    {
        float distance = GetForwardDistance();
        float sideDistance = GetSideDistance(GetSide().Invert());

        float dot = GetDot();

        if (distance < _startBrakeBeforeRotateOffset && distance > _startRotateOffset && dot < 0.5 && sideDistance > 0.5f)
            if (Math.Abs(_currentTargetSpeed - _targetRotateSpeed) > 0.01f)
            {
                _currentTargetSpeed = _targetRotateSpeed;
                _carBrakes.SetTargetSpeed(this, _targetRotateSpeed);
            }

    }

    private float GetDot()
    {
        if (_currentDestinationTransform == null)
            return 0;
        return Vector3.Dot(_carFrontPoint.forward, _currentDestinationTransform.forward);
    }
    
    private void StopRotate()
    {
        _wheels.ResetRotate();
        if (Math.Abs(_currentTargetSpeed - _targetSpeed) > 0.01f)
        {
            _currentTargetSpeed = _targetSpeed;
            _carBrakes.SetTargetSpeed(this, _targetSpeed);
        }

        _rotating = false;
    }

    private float GetForwardDistance()
    {
        Vector3 forward = _carFrontPoint.forward;
        if ( Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
        {
            return Mathf.Abs(_carFrontPoint.position.x - _currentDestination.x);
        }
        return Mathf.Abs(_carFrontPoint.position.z - _currentDestination.z);
    }
    
    private float GetSideDistance(Side rotateSide)
    {
        float distance;
        if (rotateSide == Side.Right)
            distance = Mathf.Abs(_currentDestination.z -_carFrontPoint.position.z);
        else
            distance = Mathf.Abs(_currentDestination.x - _carFrontPoint.position.x);

        return distance;
    }
    

    private Side GetSide()
    {
        return GetSide(_carFrontPoint.forward);
    }
    
    private Side GetSide(Vector3 forward, float minOffset = 0.0f)
    {
        
        float currentPosition = 0.0f;
        float targetPosition = 0.0f;
        Side side = Side.Right;

        if (Mathf.Abs(forward.x- forward.z) < minOffset)
            return Side.Forward;
        
        if ( Mathf.Abs(forward.x) > Mathf.Abs(forward.z) )
        {
            currentPosition = _carFrontPoint.position.z;
            targetPosition = _currentDestination.z;
            side = currentPosition < targetPosition ? Side.Left : Side.Right;
            if (forward.x < 0)
                side = side.Invert();
        }
        else if ( Mathf.Abs(forward.z) > Mathf.Abs(forward.x))
        {
            currentPosition = _carFrontPoint.position.x;
            targetPosition = _currentDestination.x;
            side = currentPosition > targetPosition ? Side.Left : Side.Right;
            if (forward.z < 0)
                side = side.Invert();
        }
        else
        {
            return Side.Forward;
        }

        return side;
    }

    private float GetSideCoefficient(Side side)
    {
        float coefficient = 1;
        if (side == Side.Left)
            coefficient = -1;
        else if (side == Side.Forward || side == Side.Backward)
            coefficient = 0;

        return coefficient;
    }
    
    public void SetDestination(Transform targetTransform)
    {
        _currentDestinationTransform = targetTransform;
        _currentDestination = targetTransform.position;
        StopRotate();
    }
}
