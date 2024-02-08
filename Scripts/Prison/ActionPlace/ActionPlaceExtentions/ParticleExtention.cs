using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleExtention : ActionPlaceExtention
{
    [SerializeField] private List<ParticleSystem> _particleSystems;

    private void Start()
    {
        Stop();
    }
    
    protected override void OnUsePlace(Prisoner prisoner)
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Play();
        }
    }

    protected override void OnKick(Prisoner prisoner)
    {
        Stop();
    }

    private void Stop()
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Stop();
        }
    }
}
