using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MovementPlane : MonoBehaviour
{
    [SerializeField] private List<Transform> _destinations;
    [SerializeField] private float _minimumDistance;
    
    [SerializeField] private Transform _releasedPoint;
    [SerializeField] private StackSize _size;

    [SerializeField] private List<RunPath> _runPaths;

    private Pool<Transform> _destinationsPool;
    public List<RunPath> RunPaths => _runPaths;
    public Transform ReleasedPoint => _releasedPoint;

    private void Awake()
    {
        _destinationsPool = new Pool<Transform>(_destinations);
    }

    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-_size.Size.x/2, _size.Size.x/2);
        float randomZ = Random.Range(-_size.Size.z/2, _size.Size.z/2);
        var randomPosition =  transform.position + _size.Center + new Vector3(randomX, 0, randomZ);
        
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            randomPosition = hit.position;
        }
        
        return randomPosition;
    }

    public Vector3 GetRandomPosition(Vector3 previousPosition)
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = GetRandomPosition();
        } 
        while (Vector3.Distance(previousPosition, randomPosition) > _minimumDistance);

        return randomPosition;
    }

    public Vector3 GetDestination(Transform sender)
    {
        var destination = _destinationsPool.RandomFromPool(x => Vector3.Distance(sender.position, x.position) > _minimumDistance);
        return destination.position;
    }
    
    
    
}
