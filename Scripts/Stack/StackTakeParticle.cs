using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using Zenject;

public class StackTakeParticle : MonoBehaviour
{
    [SerializeField] private StackTaker _stackTaker;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Transform _parent;
    [SerializeField] private ParticleSystem _particleSystemPrefab;

    [Inject] private Timer _timer;
    
    private void OnEnable()
    {
        _stackTaker.OnTake += OnTake;
    }

    private void OnDisable()
    {
        _stackTaker.OnTake -= OnTake;
    }

    private void OnTake(StackItem stackItem)
    {
        _timer.ExecuteWithDelay(() => Instantiate(_particleSystemPrefab, _parent), _spawnDelay);
    }
}
