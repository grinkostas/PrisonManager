using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Zenject;

public class BarbellExtention : ActionPlaceExtention
{
    [Header("Barbell")] 
    [SerializeField] private Transform _barbell;
    [SerializeField] private Transform _barbellStartPoint;
    [SerializeField] private Vector3 _barbellOffset;
    private bool _isAction = false;

    private void Update()
    {
        if(_isAction)
            _barbell.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    protected override void OnUsePlace(Prisoner prisoner)
    {
        _isAction = true;
        Vector3 barbellPosition = Vector3.Lerp(prisoner.LeftHand.position, prisoner.RightHand.position, 0.5f) + _barbellOffset;
        _barbell.position = barbellPosition;
        _barbell.SetParent(prisoner.RightHand);
    }

    protected override void OnKick(Prisoner prisoner)
    {
        _isAction = false;
        _barbell.SetParent(_barbellStartPoint);
        _barbell.localPosition = Vector3.zero;
        _barbell.localRotation = Quaternion.identity;
    }
}
