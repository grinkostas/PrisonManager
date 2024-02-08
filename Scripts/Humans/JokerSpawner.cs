using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class JokerSpawner : MonoBehaviour
{
    [SerializeField] private Human _humanPrefab;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private BuyZone _jokerRoomBuyZone;
    
    [Inject] private HumanSpawner _humanSpawner;

    private float _timer = 0.0f;
    
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval && _jokerRoomBuyZone.IsBought)
        {
            _timer = 0.0f;
            _humanSpawner.SpawnHuman(_humanPrefab);
        }
    }
}
