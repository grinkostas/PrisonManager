using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class TrashSpawnCondition : MonoBehaviour
{
    [SerializeField] private TrashSpawner _spawner;
    [SerializeField] private float _spawnTime;

    private float _timer = 0.0f;
    private void Update()
    {
        if(_spawner.CanSpawn() == false || SpawnCondition() == false)
            return;
        _timer += Time.deltaTime;
        if (_timer >= _spawnTime + GetAdditionalSpawnTime())
        {
            _timer = 0.0f;
            _spawner.Spawn();
        }
    }

    protected abstract bool SpawnCondition();
    protected abstract float GetAdditionalSpawnTime();

}
