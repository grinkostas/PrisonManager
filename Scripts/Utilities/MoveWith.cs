using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveWith : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _sourceTransform;

    private void Update()
    {
        _targetTransform.position = _sourceTransform.position;
        _targetTransform.rotation = _sourceTransform.rotation;
    }
}
