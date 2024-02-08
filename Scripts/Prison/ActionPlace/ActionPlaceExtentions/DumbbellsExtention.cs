using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;

public class DumbbellsExtention : ActionPlaceExtention
{
    [SerializeField] private GameObject _dumbbellPrefab;
    [SerializeField] private bool _leftHand = true;
    [SerializeField] private bool _rightHand = true;
    [SerializeField] private bool _overrideLocals = false;
    [SerializeField, ShowIf(nameof(_overrideLocals))] private Vector3 _localPosition;
    [SerializeField, ShowIf(nameof(_overrideLocals))] private Vector3 _leftHandLocalRotation;
    [SerializeField, ShowIf(nameof(_overrideLocals))] private Vector3 _rightHandLocalRotation;
    [SerializeField, ShowIf(nameof(_overrideLocals))] private Vector3 _scale;
    private List<GameObject> _spawnedObjects = new List<GameObject>();
    protected override void OnUsePlace(Prisoner prisoner)
    {
        if (_leftHand)
        {
            var spawnedItem = SpawnItem(prisoner.LeftHand);
            if(_overrideLocals)
                spawnedItem.transform.localRotation = quaternion.Euler(_leftHandLocalRotation);
        }

        if (_rightHand)
        {
            var spawnedItem = SpawnItem(prisoner.RightHand);
            if(_overrideLocals)
                spawnedItem.transform.localRotation = quaternion.Euler(_rightHandLocalRotation);
        }
    }

    private GameObject SpawnItem(Transform parent)
    {
        var rightDumbbell = Instantiate(_dumbbellPrefab, parent);
        if (_overrideLocals)
        {
            rightDumbbell.transform.localScale = _scale;
            rightDumbbell.transform.localPosition = _localPosition;
        }
        
        _spawnedObjects.Add(rightDumbbell);
        return rightDumbbell;
    }

    protected override void OnKick(Prisoner prisoner)
    {
        foreach (var spawnedObject in _spawnedObjects)
        {
            Destroy(spawnedObject);
        }
        _spawnedObjects.Clear();
    }
}
