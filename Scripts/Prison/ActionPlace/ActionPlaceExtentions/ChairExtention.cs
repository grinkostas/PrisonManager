using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ChairExtention : ActionPlaceExtention
{
    [SerializeField] private GameObject _plate;
    [SerializeField] private float _yOffset;
    [SerializeField] private float _zoomDuration;
    [SerializeField] private float _fallDuration;

    private void Start()
    {
        _plate.transform.localScale = Vector3.zero;
    }

    protected override void OnUsePlace(Prisoner prisoner)
    {
        _plate.transform.position += Vector3.up * _yOffset;
        _plate.transform.DOMove(_plate.transform.position - Vector3.up * _yOffset, _fallDuration);
        _plate.transform.DOScale(Vector3.one, _zoomDuration).SetEase(Ease.OutBack);
    }

    protected override void OnKick(Prisoner prisoner)
    {
        _plate.transform.DOScale(Vector3.zero, _zoomDuration);
    }

}
