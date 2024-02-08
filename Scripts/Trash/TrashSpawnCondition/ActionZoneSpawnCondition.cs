using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class ActionZoneSpawnCondition : TrashSpawnCondition
{
    [SerializeField, HideIf(nameof(_container))] private ActionZone _actionZone;
    [SerializeField, ShowIf(nameof(_container))] private ActionZoneContainer _actionZoneContainer;
    [SerializeField] private bool _container;
    [SerializeField] private int _usesToSpawn;

    private int _currentUses = 0;

    private void OnEnable()
    {
        foreach (var place in GetPlaces())
        {
            place.Used += OnUsePlace;
        }
    }

    private void OnDisable()
    {
        foreach (var place in GetPlaces())
        {
            place.Used -= OnUsePlace;
        }
    }

    
    private void OnUsePlace(Prisoner prisoner)
    {
        _currentUses++;
    }

    private List<ActionPlaceBase> GetPlaces()
    {
        if (_container)
            return _actionZoneContainer.GetAllPlaces();
        return _actionZone.Places;
    }

    protected override bool SpawnCondition()
    {
        bool result =  _currentUses >= _usesToSpawn;
        if (result)
            _currentUses -= _usesToSpawn;
        return result;
    }

    protected override float GetAdditionalSpawnTime()
    {
        return 0.0f;
    }
}
