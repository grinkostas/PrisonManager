using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class ObstaclesAvoider : MonoBehaviour
{
    [SerializeField] private List<ObstacleType> _ignoreTypes;
    [SerializeField] private List<Obstacle> _ignoreObstacles;
    [SerializeField] private bool _sameDirection;
    
    
    public List<Obstacle> _obstacles = new List<Obstacle>();

    public bool _startedAvoid = false;
    public abstract List<Transform> GetRaycastPoints();


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            Add(obstacle);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            Remove(obstacle);
        }
    }

    private void Add(Obstacle obstacle)
    {
        if(_ignoreTypes.Contains(obstacle.Type))
            return;
        if(_ignoreObstacles.Contains(obstacle))
            return;
        
        if(_obstacles.Contains(obstacle))
            return;
        
        if (_sameDirection)
        {
            float dot =Vector3.Dot(obstacle.transform.forward, transform.forward);
            if(dot <= 0f)
                return;
        }

        _obstacles.Add(obstacle);
        if (_startedAvoid == false)
        {
            _startedAvoid = true;
            StartAvoid();
        }
    }

    private void Remove(Obstacle obstacle)
    {
        _obstacles.Remove(obstacle);
        if (_obstacles.Count == 0)
        {
            StopAvoid();
            _startedAvoid = false;
        }
    }

    protected abstract void StartAvoid();
    protected abstract void StopAvoid();
}
