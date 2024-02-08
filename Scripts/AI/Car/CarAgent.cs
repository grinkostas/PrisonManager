using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarAgent : MonoBehaviour
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private List<Transform> _destinations;

    private Vector3 _destination = Vector3.positiveInfinity;

    private int _currentIndex = -1;
    
    private void OnEnable()
    {
        _carMovement.OnReceiveDestination += NextPoint;
    }

    private void Start()
    {
        NextPoint();
    }

    private void NextPoint()
    {
        _currentIndex++;
        if (_currentIndex >= _destinations.Count)
            _currentIndex = 0;
        _carMovement.SetDestination(_destinations[_currentIndex]);
    }

}
