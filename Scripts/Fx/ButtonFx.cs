using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonFx : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private float _bounceDuration;
    [SerializeField] private float _zoomMultiplayer;

    private Tweener _currentTweener;
    private Vector3 _startZoom;

    private void Awake()
    {
        _startZoom = _button.transform.localScale;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }
    private void OnClick()
    {
        if(_currentTweener != null)
            _currentTweener.Kill();
        _currentTweener = _button.transform.DOScale(_startZoom * _zoomMultiplayer, _bounceDuration / 2).OnComplete(
        () =>
        {
            _currentTweener = _button.transform.DOScale(_startZoom, _bounceDuration / 2);
        });
    }
}
