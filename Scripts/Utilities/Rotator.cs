using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Transform _rotateModel;
    [SerializeField] private float _rotationAngle;
    [SerializeField] private Vector3 _rotationAxis;
    [SerializeField] private float _oneLoopDuration;

    
    private Vector3 _startRotation;
    private Tweener _tweener;

    private void Awake()
    {
        _startRotation = _rotateModel.transform.localRotation.eulerAngles;
    }

    private void OnEnable()
    {
        _tweener = _rotateModel.DOLocalRotate(_startRotation + _rotationAxis * _rotationAngle, _oneLoopDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);
    }

    private void OnDisable()
    {
        _tweener.Kill();
    }
}
